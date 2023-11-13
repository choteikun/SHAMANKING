using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MaterialFader
{
   public static void Mat_ShaderValueFloatTo(string ShaderValueName, float curValue, float endValue, float lerpTime, Material material)
    {
        DOTween.To(() => curValue, x => curValue = x, endValue, lerpTime)
            .OnUpdate(() =>
            {
                // 在動畫更新時，可以使用 currentValue 來獲取當前的 float 值
                //Debug.Log(ghost_Stats_.Ghost_SkinnedMeshRenderer.material.name + curValue);
                //ghost_Stats_.Ghost_SkinnedMeshRenderer.material.SetFloat(ShaderValueName, curValue);
                material.SetFloat(ShaderValueName, curValue); //硬加的
            })
            .OnComplete(() =>
            {
                // 在動畫完成時執行任何需要的操作
                //Debug.Log(ShaderValueName + "Complete!");
            });
    }
    public static void Mat_ShaderValueVecterTo(string ShaderValueName, Vector3 curValue, Vector3 endValue, float lerpTime, Material material)
    {
        DOTween.To(() => curValue, x => curValue = x, endValue, lerpTime)
            .OnUpdate(() =>
            {
                // 在動畫更新時，可以使用 currentValue 來獲取當前的 float 值
                //Debug.Log(ghost_Stats_.Ghost_SkinnedMeshRenderer.material.name + curValue);
                //ghost_Stats_.Ghost_SkinnedMeshRenderer.material.SetFloat(ShaderValueName, curValue);
                material.SetVector(ShaderValueName, curValue); //硬加的
            })
            .OnComplete(() =>
            {
                // 在動畫完成時執行任何需要的操作
                //Debug.Log(ShaderValueName + "Complete!");
            });
    }
}
