using Gamemanager;
using UniRx;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerAimerView : MonoBehaviour
{
    [SerializeField]
    GameObject aimPoint_;
    [SerializeField]
    GameObject supportAimSystem_;
    [SerializeField]
    float distance_;
    [SerializeField]
    float smoothing_;
    [SerializeField]
    GameObject rayCube_;
    [SerializeField]
    GameObject nowAimimgObject_;
    [SerializeField]
    HitableItemTest hitObjectInfo_;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemGetHitableItem, cmd => supportAimSystemGetObject(cmd));
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemLeaveHitableItem, cmd => supportAimSystemLeaveObject(cmd));
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, cmd => { releaseAimButton(cmd); } );
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerLaunchActionFinish, cmd => { turnOffAimer(); });
        // GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Subscribe(cmd => { rayCube_.SetActive(cmd.AimingButtonIsPressed); });
    }
    private void Update()
    {
        aimPointUpdate();
        rayCubeUpdate(aimPoint_.transform.position);
    }

    void rayCubeUpdate(Vector3 pointB)
    {
        var pointA = Camera.main.transform.position;
        rayCube_.transform.localScale = new Vector3(0.5f, 0.5f, distance_);
        rayCube_.transform.position = (pointA + pointB) / 2f;
        Vector3 directionToB = (pointB - pointA).normalized;
        Quaternion rotation = Quaternion.LookRotation(directionToB);

        // 应用旋转角度
        rayCube_.transform.rotation = rotation;
    }
    void aimPointUpdate()
    {
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //if (Physics.Raycast(ray, out RaycastHit hit, distance_, aimColloderLayerMask))
        //{
        //    aimPoint_.transform.position = Vector3.Lerp(aimPoint_.transform.position, hit.point, smoothing_ * Time.deltaTime);
        //}
        //else
        //{
        var endPoint = ray.origin + ray.direction * distance_;
        aimPoint_.transform.position = Vector3.Lerp(aimPoint_.transform.position, endPoint, smoothing_ * Time.deltaTime);
        if (nowAimimgObject_ == null)
        {
            supportAimSystem_.transform.position = Vector3.Lerp(supportAimSystem_.transform.position, endPoint, smoothing_ * Time.deltaTime);
        }
        else
        {
            supportAimSystem_.transform.position = Vector3.Lerp(supportAimSystem_.transform.position, hitObjectInfo_.onHitPoint_.transform.position, smoothing_ * Time.deltaTime);
        }
    }
    void supportAimSystemGetObject(SupportAimSystemGetHitableItemCommand command)
    {
        nowAimimgObject_ = command.HitObject;
        hitObjectInfo_ = command.HitableItemInfo;
    }
    void supportAimSystemLeaveObject(SupportAimSystemLeaveHitableItemCommand command)
    {
        if (command.LeaveObject == nowAimimgObject_)
        {
            nowAimimgObject_ = null;
            hitObjectInfo_ = null;
        }
    }
    void releaseAimButton(PlayerAimingButtonCommand cmd)
    {
        rayCube_.SetActive(cmd.AimingButtonIsPressed);
        if (!cmd.AimingButtonIsPressed ) { hitObjectInfo_ = null; nowAimimgObject_ = null; }
    }
    void turnOffAimer()
    {
        rayCube_.SetActive(false);
       hitObjectInfo_ = null; nowAimimgObject_ = null;
    }
    
}
