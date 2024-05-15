using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Gamemanager;

public class CamUltimateAnimationEvents : MonoBehaviour
{
    Animator camAnim_;
    [SerializeField]
    GameObject[] allCamActionObj_;
    [SerializeField]
    int delayClose_;
    [SerializeField]
    GameObject[] firstWaveWeaponObject_;
    void Start()
    {
        camAnim_ = GetComponent<Animator>();
         GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerUltimatePrepareSuccess, cmd => { ultimateAttackStart(); });
    }
    public void Dolly_Rig_CamAction1_To_CamAction2()
    {
        allCamActionObj_[1].SetActive(true);
        camAnim_.CrossFadeInFixedTime("Dolly_Rig_CamAction2", 0);
    }
    public void Dolly_Rig_CamAction2_To_CamAction3()
    {
        allCamActionObj_[2].SetActive(true);
        camAnim_.CrossFadeInFixedTime("Dolly_Rig_CamAction3", 0);
    }
    public async void CloseAll_Dolly_Rig_Cam()
    {
        await UniTask.Delay(delayClose_ * 1000);
        GameManager.Instance.MainGameEvent.Send(new PlayerUltimateAttackFinishCommand());
        for (int i = 0;i< allCamActionObj_.Length; i++)
        {
            allCamActionObj_[i].SetActive(false);
        }
    }
    void ultimateAttackStart()
    {
        allCamActionObj_[0].SetActive(true);      
        camAnim_.CrossFadeInFixedTime("Dolly_Rig_CamAction1", 0);
    }
    void cancelWeapon()
    {
        for (int i = 0; i < firstWaveWeaponObject_.Length; i++)
        {
            firstWaveWeaponObject_[i].SetActive(false);
        }
    }
}
