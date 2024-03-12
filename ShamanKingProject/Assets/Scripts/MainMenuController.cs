using Datamanager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string nextSceneName_;
     private void Start()
    {
        Cursor.visible = true;
    }
    private void Update()
    {
        foreach (var gamepad in Gamepad.all)
        {
            foreach (var control in gamepad.allControls)
            {
                if (control.IsPressed())
                {
                    Debug.Log("pressed");
                    IntoNextScene();
                }
            }
        }
    }

    public void IntoNextScene()
    {
        Cursor.visible = false;
        var realTimePlayerData = GameContainer.Get<DataManager>().realTimePlayerData;
        realTimePlayerData.Refresh();
        SceneManager.LoadScene(nextSceneName_);
    }
    
}
