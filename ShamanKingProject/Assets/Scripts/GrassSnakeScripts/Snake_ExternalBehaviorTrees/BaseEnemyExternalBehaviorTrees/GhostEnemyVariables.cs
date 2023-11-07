using UnityEngine;

public class GhostEnemyVariables : MonoBehaviour
{
    public bool WanderTrigger { get { return wanderTrigger_; } set { wanderTrigger_ = value; } }
    [SerializeField]
    private bool wanderTrigger_;
    public bool UpdatePosTrigger { get { return updatePosTrigger_; } set { updatePosTrigger_ = value; } }
    [SerializeField]
    private bool updatePosTrigger_;
    public Vector3 ZombiePos { get { return zombiePos_; } set { zombiePos_ = value; } }
    [SerializeField]
    private Vector3 zombiePos_;

    
}
