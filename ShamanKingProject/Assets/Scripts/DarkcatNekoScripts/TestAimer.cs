using UnityEngine;

public class TestAimer : MonoBehaviour
{
    [SerializeField]
    GameObject aimPoint_;
    [SerializeField]
    LayerMask aimColloderLayerMask = new LayerMask();


    private void Update()
    {
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit hit, 10f, aimColloderLayerMask))
        {
            aimPoint_.transform.position = hit.point;
        }
        else
        {
            var endPoint = ray.origin + ray.direction * 10f;
            aimPoint_.transform.position = endPoint;

        }
    }
}
