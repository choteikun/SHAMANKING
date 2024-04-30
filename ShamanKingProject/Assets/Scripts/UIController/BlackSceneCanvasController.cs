using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSceneCanvasController : MonoBehaviour
{
    [SerializeField] Animator sceneAnimator_;
    void Start()
    {
        sceneAnimator_.CrossFadeInFixedTime("IntoScene", 0.1f);
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnSystemCallSceneFadeOut, cmd => { sceneAnimator_.CrossFadeInFixedTime("OutScene", 0.1f); });
    }

    
}
