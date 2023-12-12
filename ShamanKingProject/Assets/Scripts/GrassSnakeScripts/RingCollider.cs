using PixelCrushers.DialogueSystem.UnityGUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class RingCollider : MonoBehaviour
{
    [Header("Physic")]
    [SerializeField] SphereCollider sphereCollider_ = null;
    [SerializeField] Vector2 scaleRange_ = new Vector2(0f, 10f);
    [SerializeField] AnimationCurve spreadCurve_ = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    [SerializeField] float duration_ = 5f;

    [Header("Vision")]
    [SerializeField] ParticleSystem vfx_ = null;

    [Header("Game's")]
    [SerializeField] float m_Damage = 100f;

    private IEnumerator StretchSize()
    {
        float startTime = Time.timeSinceLevelLoad;
        float endTime = startTime + duration_;
        while (Time.timeSinceLevelLoad < endTime)
        {
            float pass = Time.timeSinceLevelLoad - startTime;
            float pt = pass / duration_;
            sphereCollider_.radius = spreadCurve_.Evaluate(pt);
            yield return null;
        }
        Despawn();
    }
    private void OnEnable()
    {
        Reinit();
        StartCoroutine(StretchSize());
    }

    private void OnDisable()
    {
        Despawn();
    }
    private void Reinit()
    {
        sphereCollider_.radius = float.Epsilon;
    }
    private void Despawn()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }
}
