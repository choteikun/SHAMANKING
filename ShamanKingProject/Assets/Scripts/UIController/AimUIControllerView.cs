using Gamemanager;
using UnityEngine;
using UnityEngine.UI;

public class AimUIControllerView : MonoBehaviour
{
    [SerializeField]
    Image aimPointImage_;
    [SerializeField]
    Canvas aimCanvas_;
    [SerializeField]
    Image supportAimPointImage_;
    [SerializeField]
    GameObject nowSupportAimingObject_;
    RectTransform canvasRectTransform_;

    private void Start()
    {
        canvasRectTransform_ = aimCanvas_.GetComponent<RectTransform>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemGetHitableItem, cmd => { supportAimSystemGetHitableItem(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSupportAimSystemLeaveHitableItem, cmd => { supportAimSystemLeaveHitableItem(cmd); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnAimingButtonTrigger, cmd => { aimingButtonTrigger(cmd); });
    }
    private void Update()
    {
        supportAimSystemHintImageUpdate();
    }

    void supportAimSystemHintImageUpdate()
    {
        if (nowSupportAimingObject_ == null) return;
        var supportAimObjectToRectTransform = Camera.main.WorldToScreenPoint(nowSupportAimingObject_.transform.position);
        Vector2 localPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform_, supportAimObjectToRectTransform, null, out localPosition))
        {
            // 设置Image的RectTransform的局部坐标，使其中心与目标对象的位置对齐
            supportAimPointImage_.rectTransform.anchoredPosition = localPosition;
            //Debug.Log(localPosition);
        }
        //var rect = new Vector2((supportAimObjectToRectTransform.x - 0.5f) * 960, (supportAimObjectToRectTransform.y - 0.5f) * 540);
        //supportAimPointImage_.rectTransform.anchoredPosition = rect;
    }

    void supportAimSystemGetHitableItem(SupportAimSystemGetHitableItemCommand cmd)
    {
        supportAimPointImage_.gameObject.SetActive(true);
        aimPointImage_.gameObject.SetActive(false);
        nowSupportAimingObject_ = cmd.HitableItemInfo.onHitPoint_;
    }

    void supportAimSystemLeaveHitableItem(SupportAimSystemLeaveHitableItemCommand cmd)
    {
        supportAimPointImage_.gameObject.SetActive(false);
        aimPointImage_.gameObject.SetActive(true);
        nowSupportAimingObject_ = null;
    }

    void aimingButtonTrigger(PlayerAimingButtonCommand cmd)
    {
        if (!cmd.AimingButtonIsPressed)
        {
            aimPointImage_.gameObject.SetActive(false);
            supportAimPointImage_.gameObject.SetActive(false);
            nowSupportAimingObject_ = null;
        }
        else
        {
            aimPointImage_.gameObject.SetActive(true);
        }
    }
}
