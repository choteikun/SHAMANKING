using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using Obi;
using UniRx;
using UnityEngine;

public class GhostLauncherView : MonoBehaviour
{
    [SerializeField]
    GameObject ghostLaunchFollowTarget_;
    [SerializeField]
    GameObject aimingFollowPoint_;
    [SerializeField]
    GameObject aimingTarget_;
    [SerializeField]
    ObiRopeCursor ropeCursor_;
    [SerializeField]
    ObiRope rope_;

    [SerializeField]
    float basicLength_;
    [SerializeField]
    float ropeLength_;

    [SerializeField]
    float launchSpeed_;

    [SerializeField]
    HitableItemTest nowAimingObjectHitInfo;

    Tweener aimTargetEvent_;
    Tweener ropeExtrudeEvent_;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemGetHitableItem, cmd => { nowAimingObjectHitInfo = cmd.HitableItemInfo; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemLeaveHitableItem, cmd => { nowAimingObjectHitInfo = null; });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd => { onLaunchStart(); });
        var playerLaunchActionFinishEvent = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit).Subscribe(cmd => { stopLaunchTweener(); });
        var GhostLaunchProcessFinishEvent = GameManager.Instance.MainGameEvent.OnGhostLaunchProcessFinish.Subscribe(cmd => { ropeLength_ = 0; });
        var AimingButtonTriggerEvent = GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => !cmd.AimingButtonIsPressed).Subscribe(cmd => { nowAimingObjectHitInfo = null; });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && (cmd.HitObjecctTag == HitObjecctTag.Possessable || cmd.HitObjecctTag == HitObjecctTag.Biteable)).Subscribe(cmd => setRopeLengthByOtherObject(cmd.HitInfo.onHitPoint_.transform));

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

        var launchTime = getLaunchTimeByDistance(length);

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
        ropeCursor_.ChangeLength(basicLength_ + ropeLength_);
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
    float getLaunchTimeByDistance(float distance)
    {
        var percentage = distance / 20f;
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
        GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = true, HitObjecctTag = nowAimingObjectHitInfo.HitTag, HitInfo = nowAimingObjectHitInfo,HitObjecct = nowAimingObjectHitInfo.gameObject });

    }
}
