using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneCameraTransfer : MonoBehaviour
{
    [SerializeField] GameObject mainCmCamera_;
    [SerializeField] GameObject[] cinematicCameras;
    [SerializeField] int nowActivatedCamera = -1;
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallFirstSceneCameraTransfer, cmd => { setUpCinematic((int)cmd.CameraId); });
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallCameraTransferBack, cmd => { backToMainCamera(); });
    }
    void setUpCinematic(int cameraId)
    {
        mainCmCamera_.SetActive(false);
        cinematicCameras[cameraId].SetActive(true);
        nowActivatedCamera = cameraId;
    }

    void backToMainCamera()
    {
        cinematicCameras[nowActivatedCamera].SetActive(false);
        nowActivatedCamera = -1;
        mainCmCamera_ .SetActive(true);
    }
}
