using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayItem : MonoBehaviour
{
    /// <summary>
    /// 定時銷毀殘影
    /// 並且帶漸漸消隱效果
    /// </summary>

    //持續時間
    public float duration;
    //銷毀時間
    public float deleteTime;
    //物體上的 MeshRenderer ，主要是為了 動態修改材質顏色 alpha 值，產生漸隱效果
    public MeshRenderer meshRenderer;
    void Update()
    {

        float tempTime = deleteTime - Time.time;

        if (tempTime <= 0)
        {//時間到就銷毀
            GameObject.Destroy(this.gameObject);
        }
        else if (meshRenderer.material)
        {
            //這裡根據所剩時間的比例，來產生殘影漸隱的效果
            float rate = tempTime / duration;//計算生命週期的比例
            //Color cal = meshRenderer.material.GetColor("_BaseColor");
            //cal.a *= rate;//設置透明通道
            //Debug.LogError(rate);
            //meshRenderer.material.SetFloat("_Dissolve", rate);
            meshRenderer.material.SetFloat("_Alpha", rate);
        }

    }
}
