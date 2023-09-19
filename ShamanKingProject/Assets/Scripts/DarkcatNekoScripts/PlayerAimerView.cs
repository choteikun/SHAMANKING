using System.Net;
using UnityEngine;

public class PlayerAimerView : MonoBehaviour
{
    [SerializeField]
    GameObject aimPoint_;
    [SerializeField]
    LayerMask aimColloderLayerMask = new LayerMask();
    [SerializeField]
    float distance_;
    [SerializeField]
    float smoothing_;
    private void Update()
    {
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, distance_, aimColloderLayerMask))
        {
            aimPoint_.transform.position = Vector3.Lerp(aimPoint_.transform.position, hit.point, smoothing_ * Time.deltaTime);
        }
        else
        {
            var endPoint = ray.origin + ray.direction * distance_;
            aimPoint_.transform.position = Vector3.Lerp(aimPoint_.transform.position, endPoint, smoothing_ * Time.deltaTime);
        }
    }
}
