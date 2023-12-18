using UnityEngine;
using UniRx;
using Gamemanager;

public class EnemyLockOn : MonoBehaviour
{
    Transform currentTarget;
    //Animator anim;
    [SerializeField] GameObject enemyTarget_Locator;
    [SerializeField] LayerMask targetLayers;

    //[Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float playerKeepUpRange_ = 2f;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")][SerializeField] float maxNoticeAngle = 60;

    Transform cam;
    [SerializeField] Transform targetCameraFollowedObject_;
    bool enemyLocked;
    float currentYOffset;
    Vector3 pos;

    [SerializeField] GameObject gizmosUsedHip_;

    void Start()
    {
        cam = Camera.main.transform;
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerTargetButtonTrigger, cmd => { triggerTargetButton(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLightAttack, cmd => { checkIfInAttackRangeAttackRange(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerThrowAttack, cmd => { checkIfInAttackRangeAttackRange(); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerHeavyAttack, cmd => { checkIfInAttackRangeAttackRange(); });       
    }

    void Update()
    {
        if (enemyLocked)
        {
            if (!TargetOnRange()) ResetTarget();
            LookAtTarget();
        }

    }

    void triggerTargetButton()
    {
        if (currentTarget)
        {
            //If there is already a target, Reset.
            ResetTarget();
            return;
        }
        currentTarget = ScanNearBy();
        if (currentTarget != null)
        {
            FoundTarget();
        }
        else
        {
            ResetTarget();
        }
    }

    void FoundTarget()
    {
        enemyLocked = true;
        GameManager.Instance.MainGameEvent.Send(new SystemGetTarget() { Target = currentTarget.gameObject });
    }

    void ResetTarget()
    {
        Debug.Log("Cancel");
        currentTarget = null;
        enemyLocked = false;
        GameManager.Instance.MainGameEvent.Send(new SystemResetTarget());
    }

    void checkIfInAttackRangeAttackRange()
    {
        if (enemyLocked)
        {
            float dis = (transform.position - pos).magnitude;
            if (dis  > playerKeepUpRange_)
            {
                GameManager.Instance.MainGameEvent.Send(new AnimationMovementEnableCommand());
            }
            else
            {
                GameManager.Instance.MainGameEvent.Send(new AnimationMovementDisableCommand());
            }
        }
        else
        {
            GameManager.Instance.MainGameEvent.Send(new AnimationMovementEnableCommand());
        }
    }
    private Transform ScanNearBy()
    {
        Debug.Log("Finding");
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;
        if (nearbyTargets.Length <= 0)
        { Debug.Log("Cant find"); return null; }
        Debug.Log("Finding" + nearbyTargets.Length.ToString());
        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - cam.position;
            dir.y = 0;
            float _angle = Vector3.Angle(cam.forward, dir);

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                closestAngle = _angle;
            }
        }
        Debug.Log("Finding" + closestTarget.name.ToString());
        if (!closestTarget)
        { Debug.Log("Cant find"); return null; }
        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;
        if (zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3) currentYOffset = 1.6f;
        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);
        if (Blocked(tarPos))
        { Debug.Log("Cant find"); return null; }
        Debug.Log("Finding" + closestTarget.name.ToString());
        pos = closestTarget.position + new Vector3(0, currentYOffset, 0);
        return closestTarget;
    }

    bool Blocked(Vector3 t)
    {
        RaycastHit hit;
        if (Physics.Linecast(transform.position + Vector3.up * 0.5f, t, out hit))
        {
            if (hit.transform.CompareTag("Object")) return true;
        }
        return false;
    }

    bool TargetOnRange()
    {
        float dis = (transform.position - pos).magnitude;
        if (dis /2 > noticeZone) return false; else return true;
    }


    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget();
            return;
        }
        pos = currentTarget.position + new Vector3(0, currentYOffset, 0);
        //lockOnCanvas.position = pos;
        //lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale);

        enemyTarget_Locator.transform.position = pos;
        Vector3 dir = currentTarget.position - targetCameraFollowedObject_.transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        targetCameraFollowedObject_.transform.rotation = Quaternion.Lerp(targetCameraFollowedObject_.transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, playerKeepUpRange_);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gizmosUsedHip_.transform.position, playerKeepUpRange_);
    }
}
