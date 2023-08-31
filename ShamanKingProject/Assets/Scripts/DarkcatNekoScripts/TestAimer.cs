using UnityEngine;

public class TestAimer : MonoBehaviour
{
    [SerializeField]
    GameObject aimPoint_;
    [SerializeField]
    LayerMask aimColloderLayerMask = new LayerMask();
    [SerializeField]
    float distance_;

    private void FixedUpdate()
    {
        
    }
    private void Update()
    {
        var screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        //if (Physics.Raycast(ray, out RaycastHit hit, distance_, aimColloderLayerMask))
        //{
        //    aimPoint_.transform.position = hit.point;
        //}
        //else
        //{
            var endPoint = ray.origin + ray.direction * distance_;
            aimPoint_.transform.position = endPoint;

        //}
    }
}
