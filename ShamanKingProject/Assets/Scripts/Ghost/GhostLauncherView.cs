using UnityEngine;
using DG.Tweening;
using UniRx;
public class GhostLauncherView : MonoBehaviour
{
    [SerializeField]
    GameObject ghostLaunchFollowTarget_;
    [SerializeField]
    GameObject aimingFollowPoint_;
    [SerializeField]
    GameObject aimingTarget_;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchGhost,cmd=>{ onLaunchStart(); } );
    }
    void onLaunchStart()
    {
        ghostLaunchFollowTarget_.transform.position = aimingFollowPoint_.transform.position;
        ghostLaunchFollowTarget_.transform.rotation = aimingFollowPoint_.transform.rotation;
        ghostLaunchFollowTarget_.transform.DOMove(aimingTarget_.transform.position, 3f);
    }
}
