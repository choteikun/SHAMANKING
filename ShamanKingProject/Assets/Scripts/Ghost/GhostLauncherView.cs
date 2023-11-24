using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using Obi;
using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostLauncherView : MonoBehaviour
{

    [SerializeField] GameObject ghostMassPoint_;
    [SerializeField] GameObject girlMassPoint_;
    [SerializeField]
    GameObject ghostLaunchFollowTarget_;
    [SerializeField]
    GameObject aimingFollowPoint_;
    [SerializeField]
    GameObject aimingTarget_;
    [SerializeField]
    GameObject horizontalPlayerAimObject_;
    [SerializeField]
    ObiRopeCursor ropeCursor_;
    [SerializeField]
    ObiRope rope_;
    [SerializeField]
    GameObject ghostFollowRestPoint_;

    [SerializeField]
    float basicLength_;
    [SerializeField]
    float ropeLength_;

    [SerializeField]
    float launchSpeed_;

    [SerializeField]
    HitableItemTest nowAimingObjectHitInfo;

    [SerializeField] GameObject throwAttackStartPoint_;
    [SerializeField] GameObject throwAttackFollowTarget_;

    Tweener aimTargetEvent_;
    Tweener ropeExtrudeEvent_;

    [SerializeField] bool attacking_ = false;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemAttackAllow, cmd => { ropeLength_ = 1.5f; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerMovementInterruptionFinish, cmd => { ropeLength_ = 0; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemGetHitableItem, cmd => { nowAimingObjectHitInfo = cmd.HitableItemInfo; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemLeaveHitableItem, cmd => { nowAimingObjectHitInfo = null; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd => { onLaunchStart(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttackFinish, cmd => { stopLaunchTweener(); DOTween.To(() => ropeLength_, x => ropeLength_ = x, 0, 0.2f); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccess, cmd => { stopLaunchTweener(); setRopeLengthByOtherObject(cmd.AttackTarget.transform); });
        var playerLaunchActionFinishEvent = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit).Subscribe(cmd => { stopLaunchTweener(); });
        var GhostLaunchProcessFinishEvent = GameManager.Instance.MainGameEvent.OnGhostLaunchProcessFinish.Subscribe(cmd =>
        {
            ropeLength_ = 0;
        });
        var AimingButtonTriggerEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => !cmd.AimingButtonIsPressed).Subscribe(cmd => { nowAimingObjectHitInfo = null; });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Possessable || cmd.HitObjecctTag == HitObjecctTag.Biteable)).Subscribe(cmd => setRopeLengthByOtherObject(cmd.HitInfo.onHitPoint_.transform));

        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "PlayerThrowAttackReady")
            {
                onThrowStart();
            }
        });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerAnimationEvents, cmd =>
        {
            if (cmd.AnimationEventName == "Player_Pull_Finish")
            {
                stopLaunchTweener(); //ropeLength_ = 0;
            }
        });

        GameManager.Instance.MainGameMediator.AddToDisposables(playerLaunchActionFinishEvent);
        GameManager.Instance.MainGameMediator.AddToDisposables(GhostLaunchProcessFinishEvent);
        GameManager.Instance.MainGameMediator.AddToDisposables(AimingButtonTriggerEvent);
        GameManager.Instance.MainGameMediator.AddToDisposables(onLaunchHitPosscessableItem);
        basicLength_ = rope_.restLength;
        //Debug.Log(rope_.restLength + "CM");
    }
    async void onLaunchStart()
    {
        ghostLaunchFollowTarget_.transform.position = aimingFollowPoint_.transform.position;
        ghostLaunchFollowTarget_.transform.rotation = aimingFollowPoint_.transform.rotation;

        await UniTask.DelayFrame(10);//為了讓甩槍不會這麼飄

        var length = (aimingTarget_.transform.position - ghostLaunchFollowTarget_.transform.position).magnitude;

        var launchTime = getLaunchTimeByDistance(length, 20);

        aimTargetEvent_ = ghostLaunchFollowTarget_.transform.DOMove(aimingTarget_.transform.position, launchTime);
        ropeExtrudeEvent_ = DOTween.To(() => ropeLength_, x => ropeLength_ = x, length, launchTime).OnComplete(
            () =>
            {
                sendLaunchFinishMessage();
            }
            );
    }
    private void Update()
    {
        //if (attacking_)
        //{
        //    var length = (ghostMassPoint_.transform.position - girlMassPoint_.transform.position).magnitude;
        //    length = Mathf.Abs(length);
        //    length = Mathf.Clamp(length, 0.25f, 10000);
        //    ropeCursor_.ChangeLength(basicLength_ + length);
        //}
        //else
        //{

        ropeCursor_.ChangeLength(basicLength_ + ropeLength_);
        //}
        //Debug.Log(rope_.restLength);
    }

    void stopLaunchTweener()
    {
        aimTargetEvent_.Kill();
        ropeExtrudeEvent_.Kill();
    }

    void setRopeLengthByOtherObject(Transform target)
    {
        var length = (target.transform.position - aimingFollowPoint_.transform.position).magnitude;
        ropeLength_ = length - basicLength_;
        ropeLength_ = Mathf.Clamp(ropeLength_, 0.25f, 10000);
    }
    float getLaunchTimeByDistance(float distance, float maxRange)
    {
        var percentage = distance / maxRange;
        var result = percentage * launchSpeed_;
        return result + 0.2f;
    }
    void sendLaunchFinishMessage()
    {
        if (nowAimingObjectHitInfo == null)
        {
            GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = false });
            return;
        }
        GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = true, HitObjecctTag = nowAimingObjectHitInfo.HitTag, HitInfo = nowAimingObjectHitInfo, HitObjecct = nowAimingObjectHitInfo.gameObject });

    }

    void onThrowStart()
    {
        throwAttackFollowTarget_.transform.position = throwAttackStartPoint_.transform.position;
        throwAttackFollowTarget_.transform.rotation = throwAttackStartPoint_.transform.rotation;

        //確認路徑


        var length = (horizontalPlayerAimObject_.transform.position - throwAttackFollowTarget_.transform.position).magnitude / 2;


        aimTargetEvent_ = throwAttackFollowTarget_.transform.DOMove(horizontalPlayerAimObject_.transform.position, 0.2f);
        ropeExtrudeEvent_ = DOTween.To(() => ropeLength_, x => ropeLength_ = x, length, 0.2f).OnComplete(
            () =>
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerThrowAttackFinishCommand());
                GameManager.Instance.MainGameEvent.Send(new PlayerThrowAttackCallHitBoxCommand() { CallOrCancel = false });
                throwAttackFollowTarget_.transform.DOMove(ghostFollowRestPoint_.transform.position, 0.3f);
            }
            );
    }
}
