using BehaviorDesigner.Runtime;
using UnityEngine;

public class GhostEnemyVariables : MonoBehaviour
{
    public bool WanderTrigger { get { return wanderTrigger_; } set { wanderTrigger_ = value; } }
    [SerializeField]
    private bool wanderTrigger_;
    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField]
    private bool updatePosTrigger_;
    public bool StunTrigger { get { return stunTrigger_; } set { stunTrigger_ = value; } }
    [SerializeField]
    private bool stunTrigger_;

    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerGrabSuccessForPlayer, cmd => { stunTrigger_ = true; });
    }

}
