using DG.Tweening;
using Obi;
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

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost, cmd => { onLaunchStart(); });
        basicLength_ = rope_.restLength;
        //Debug.Log(rope_.restLength + "CM");
    }
    void onLaunchStart()
    {
        ghostLaunchFollowTarget_.transform.position = aimingFollowPoint_.transform.position;
        ghostLaunchFollowTarget_.transform.rotation = aimingFollowPoint_.transform.rotation;
        ghostLaunchFollowTarget_.transform.DOMove(aimingTarget_.transform.position, 3f);
        var length = (aimingTarget_.transform.position - ghostLaunchFollowTarget_.transform.position).magnitude;
        DOTween.To(() => ropeLength_, x => ropeLength_ = x, length, 3);

    }
    private void Update()
    {
        ropeCursor_.ChangeLength(basicLength_+ropeLength_);
        Debug.Log(rope_.restLength);
    }
}
