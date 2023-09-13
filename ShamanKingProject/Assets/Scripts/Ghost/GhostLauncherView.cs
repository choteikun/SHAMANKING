using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamemanager;
using Obi;
using UnityEngine;
using UniRx;

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
        GameManager.Instance.MainGameEvent.OnPlayerLaunchFinish.Where(cmd => cmd.Hit).Subscribe(cmd => { stopLaunchTweener(); });
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
                GameManager.Instance.MainGameEvent.Send(new PlayerLaunchFinishCommand() { Hit = false });
                ropeLength_ = 0;
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
}
