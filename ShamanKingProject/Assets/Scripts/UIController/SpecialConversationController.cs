using UnityEngine;
using UnityEngine.UI;

public class SpecialConversationController : MonoBehaviour
{
    [SerializeField] Image specialConversation_;
    [SerializeField] GameObject uncleObj_;
    [SerializeField] RectTransform canvasRectTransform_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallSpecialConversation, cmd => { updateBlockPos(); });
    }

    void updateBlockPos()
    {
        specialConversation_.gameObject.SetActive(true);
        var supportAimObjectToRectTransform = Camera.main.WorldToScreenPoint(uncleObj_.transform.position);
        Vector2 localPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform_, supportAimObjectToRectTransform, null, out localPosition))
        {
            // 设置Image的RectTransform的局部坐标，使其中心与目标对象的位置对齐
            specialConversation_.rectTransform.anchoredPosition = localPosition;
            //Debug.Log(localPosition);
        }
    }
}
