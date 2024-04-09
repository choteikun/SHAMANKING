using DG.Tweening;
using OccaSoftware.RadialBlur.Runtime;
using UnityEngine;
using UnityEngine.Rendering;

//namespace OccaSoftware.RadialBlur.Demo
//{
public class SetRadialBlurExamples : MonoBehaviour
{
    RadialBlurPostProcess radialBlur = null;
    public Volume volume;
    private VolumeProfile profile = null;

    private void Awake()
    {
        Debug.Log("Awake");
        ValidateRadialBlurManager();
    }
    private void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnPlayerExecuteCamFeedBack, cmd => { blurAnimation(); });
        Debug.Log("Start");
    }

    private bool ValidateRadialBlurManager()
    {
        profile = volume.sharedProfile;
        return profile.TryGet(out radialBlur);
    }

    public void SetDelay(float delay)
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetDelay(delay);
    }

    public void SetIntensity(float value)
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetIntensity(value);
    }

    public void SetCenter(Vector2 value)
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetCenter(value);
    }

    public void ResetCenter()
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetCenter(Vector2.zero);
    }

    public void SetCenterFromScreenPoint(Vector2 value)
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetCenterFromScreenPoint(value);
    }

    public void SetSamples(int value)
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetSampleCount(value);
    }

    public void SetSamplesFromFloat(float value)
    {
        if (ValidateRadialBlurManager())
            radialBlur.SetSampleCount((int)value);
    }

    void blurAnimation()
    {
        Debug.Log("StartblurAnimation");
        DOTween.Sequence()
            .Append(DOTween.To(() => 0f, x => SetIntensity(x), 1.5f, 0.2f))
            .Append(DOTween.To(() => 1.5f, x => SetIntensity(x), 0f, 0.2f));

    }

}
//}
