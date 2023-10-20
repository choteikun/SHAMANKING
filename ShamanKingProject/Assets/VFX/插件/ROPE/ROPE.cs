using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeObiRopeMaterial : MonoBehaviour
{
    public Material newMaterial; // Assign the new material in the Inspector
    private MeshRenderer ropeMeshRenderer;

    private void Start()
    {
        ropeMeshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeMaterial()
    {
        if (ropeMeshRenderer != null)
        {
            ropeMeshRenderer.material = newMaterial;
        }
    }
}
