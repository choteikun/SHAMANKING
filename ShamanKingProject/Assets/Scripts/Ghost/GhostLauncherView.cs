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

    Tweener aimTargetEvent_;
    Tweener ropeExtrudeEvent_;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd => { onLaunchStart(); });
        GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit).Subscribe(cmd => { stopLaunchTweener(); });
        GameManager.Instance.MainGameEvent.OnGhostLaunchProcessFinish.Subscribe(cmd => { ropeLength_ = 0; });
        var onLaunchHitPosscessableItem = GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish.Where(cmd => cmd.Hit && cmd.HitObjecctTag == HitObjecctTag.Possessable).Subscribe(cmd => setRopeLengthByOtherObject(cmd.HitInfo.onHitPoint_.transform));
        basicLength_ = rope_.restLength;
        //Debug.Log(rope_.restLength + "CM");
    }
    async void onLaunchStart()
    {
        ghostLaunchFollowTarget_.transform.position = aimingFollowPoint_.transform.position;
        ghostLaunchFollowTarget_.transform.rotation = aimingFollowPoint_.transform.rotation;
        await UniTask.DelayFrame(10);//為了讓甩槍不會這麼飄
        aimTargetEvent_ = ghostLaunchFollowTarget_.transform.DOMove(aimingTarget_.transform.position, launchSpeed_);
        var length = (aimingTarget_.transform.position - ghostLaunchFollowTarget_.transform.position).magnitude;
        ropeExtrudeEvent_ = DOTween.To(() => ropeLength_, x => ropeLength_ = x, length, launchSpeed_).OnComplete(
            () =>
            {
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchActionFinishCommand() { Hit = false });
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
        ropeLength_ = length-basicLength_;
    }
}
