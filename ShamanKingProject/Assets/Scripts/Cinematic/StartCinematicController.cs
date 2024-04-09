using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class StartCinematicController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] GameObject rawImage_;
    [SerializeField] VideoPlayer player_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallCinematicPlay, cmd => { playCinematic(cmd.CinematicID); });
    }

    async void playCinematic(float id)
    {
        switch (id)
        {
            case 0:
                animator.CrossFadeInFixedTime("Cinematic1", 0);
                break;
            case 1:
                animator.CrossFadeInFixedTime("Cinematic2", 0);
                break;
            case 2:
                animator.CrossFadeInFixedTime("Cinematic3", 0);
                break;
            case 3:
                animator.CrossFadeInFixedTime("Cinematic4", 0);
                break;
            case 4:
                animator.CrossFadeInFixedTime("Cinematic5", 0);
                break;
            case 5:
                animator.CrossFadeInFixedTime("Cinematic6", 0);
                break;
            case 6:
                rawImage_.SetActive(true);
                player_.Play();
                await UniTask.Delay(35000);
                SceneManager.LoadScene("0301LightTEST");
                break;
            default:
                break;
        }
    }
}
