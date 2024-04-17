using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneF1CameraControl : MonoBehaviour
{
    [SerializeField] GameObject mainCMCam_;
    [SerializeField] GameObject eliteGhostUseCMCam_;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            mainCMCam_.SetActive(false);
            eliteGhostUseCMCam_.SetActive(true);
        }
    }

    public void CamReplace()
    {
        mainCMCam_.SetActive(true);
        eliteGhostUseCMCam_.SetActive(false);
    }
}
