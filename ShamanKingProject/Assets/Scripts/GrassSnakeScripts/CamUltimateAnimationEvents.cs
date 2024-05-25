using Cysharp.Threading.Tasks;
using Gamemanager;
using UnityEngine;

public class CamUltimateAnimationEvents : MonoBehaviour
{
    Animator camAnim_;
    [SerializeField]
    GameObject[] allCamActionObj_;
    [SerializeField]
    float delayClose_;
    [SerializeField]
    GameObject[] firstWaveWeaponObject_;
    [SerializeField]
    GameObject attackCollider1;
    [SerializeField]
    GameObject attackCollider2;
    [SerializeField]
    GameObject attackPoint_;

    void Start()
    {
        camAnim_ = GetComponent<Animator>();
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnCallUltimateTransferStart, cmd => {
           

            ultimateAttackStart(); 
            
        });
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
        await UniTask.Delay((int)(delayClose_ * 1000f));
        GameManager.Instance.MainGameEvent.Send(new PlayerUltimateAttackFinishCommand());
        for (int i = 0; i < allCamActionObj_.Length; i++)
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
    void spawnAttackCollider1()
    {
        var collider = Instantiate(attackCollider1, attackPoint_.transform.position, Quaternion.identity);
    }
    void spawnAttackCollider2()
    {
        var collider = Instantiate(attackCollider2, attackPoint_.transform.position, Quaternion.identity);
    }
    void camFeedBack()
    {
        GameManager.Instance.MainGameEvent.Send(new PlayerExecuteCamFeedBackCommand());
    }
}
