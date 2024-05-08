using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChanger : MonoBehaviour
{
    [SerializeField] NowGameplayType gameplayType_;
    [SerializeField] Image dialogueBG_;
    [SerializeField] Sprite[] inputType_;
    // Update is called once per frame
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallInputTypeChange, cmd => { gameplayType_ = cmd.GameplayType; });
    }
    void Update()
    {
        switch (gameplayType_)
        {
            case NowGameplayType.PlayStation:
                dialogueBG_.sprite = inputType_[0];
                break;
            case NowGameplayType.XBox:
                dialogueBG_.sprite = inputType_[1];
                break;
            case NowGameplayType.Keyboard:
                dialogueBG_.sprite = inputType_[2];
                break;
            default:
                break;
        }
    }
}
