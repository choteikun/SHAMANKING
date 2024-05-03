using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class HeadTo : MonoBehaviour
{
    public Material FaceMaterial;
    private void SetHeadDirection()
    {
        if (this.FaceMaterial !=null)
        {
            this.FaceMaterial.SetVector("_HeadForward", this.transform.forward);
            this.FaceMaterial.SetVector("_HeadRight", this.transform.right);
        }

    }
   

    private void Update()
    {
        this.SetHeadDirection();
    }
}
