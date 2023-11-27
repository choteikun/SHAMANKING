using Datamanager;
using Gamemanager;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameIniter : MonoBehaviour
{
    [SerializeField] string sceneName_;
    private async void Awake()
    {
        var data = GameContainer.Get<DataManager>();
        await data.InitDataMananger();
        ChangeScene();
    }
    public void ChangeScene()
    {         
        SceneManager.LoadScene(sceneName_);
    }
}
