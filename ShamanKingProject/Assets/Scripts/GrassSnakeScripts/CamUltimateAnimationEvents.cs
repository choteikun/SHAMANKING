using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamUltimateAnimationEvents : MonoBehaviour
{
    Animator camAnim_;
    [SerializeField]
    GameObject[] allCamActionObj_;
    void Start()
    {
        camAnim_ = GetComponent<Animator>();
         GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerUltimatePrepareSuccess, cmd => { ultimateAttackStart(); });
    }
    public void Dolly_Rig_CamAction1_To_CamAction2()
    {
        camAnim_.CrossFadeInFixedTime("Dolly_Rig_CamAction2", 0);
    }
    public void Dolly_Rig_CamAction2_To_CamAction3()
    {
        camAnim_.CrossFadeInFixedTime("Dolly_Rig_CamAction3", 0);
    }
    public void CloseAll_Dolly_Rig_Cam()
    {
        for(int i = 0;i< allCamActionObj_.Length; i++)
        {
            allCamActionObj_[i].SetActive(false);
        }
    }
    void ultimateAttackStart()
    {
        allCamActionObj_[0].SetActive(true);
        camAnim_.CrossFadeInFixedTime("Dolly_Rig_CamAction1", 0);
    }
}
