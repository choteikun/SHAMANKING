using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRayShadow : MonoBehaviour
{

    // 設置殘影的顏色
    [ColorUsage(true, true)]
    public Color ShadowColor;
    //殘影大小
    public float ShadowScale;
    //殘影持續時間
    public float ShadowDuration = 2f;
    
    //符合要求後，多少時間間隔創建線的殘影
    public float ShadowInterval = 0.1f;

    //決定生成殘影的開關
    public bool ShadowTrigger;

    //模型所擁有的網格數據
    SkinnedMeshRenderer[] meshRender;

    // 使用 X-ray shader
    Shader xRayShader;

    void Start()
    {
        //GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnStartRollMovementAnimation, cmd => { ShadowTrigger(); });
        // 獲取模型身上所有的 SkinnedMeshRenderer
        meshRender = this.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
        // 並獲取 X-ray shader
        xRayShader = Shader.Find("Shader Graphs/crastal_disslove");
    }

    // 計時，和位置參數
    private float lastTime = 0;
    private Vector3 lastPos = Vector3.zero;

    void Update()
    {
        //人物有位移才創建殘影，不然不創建
        if (lastPos == this.transform.position)
        {
            return;
        }

        lastPos = this.transform.position;

        // 更新時間在滿足間隔的時間創建
        if (Time.time - lastTime < ShadowInterval)
        {//殘影間隔時間
            return;
        }
        lastTime = Time.time;

        // 如果沒有 SkinnedMeshRenderer 返回，不必創建殘影
        if (meshRender == null) 
        {
            return;
        }
        if (!ShadowTrigger)//按Space鍵才會創建殘影
        {
            return;
        }
        
        // 創建所有 SkinnedMeshRenderer 的殘影
        for (int i = 0; i < meshRender.Length; i++)
        {
            //創建 Mesh ,並烘焙 SkinnedMeshRenderer 的mesh(網格數據)
            Mesh mesh = new Mesh();
            meshRender[i].BakeMesh(mesh);

            //創建物體，並設置不在場景中顯示
            GameObject go = new GameObject();
            //go.hideFlags = HideFlags.HideAndDontSave;

            //在物體上掛載定時銷毀腳本，設置銷毀時間
            XRayItem item = go.AddComponent<XRayItem>();
            item.duration = ShadowDuration;
            item.deleteTime = Time.time + ShadowDuration;

            //在物體上添加 MeshFilter 組件，並網格數據給MeshFilter
            MeshFilter filter = go.AddComponent<MeshFilter>();
            filter.mesh = mesh;

            //在物體上添加 MeshRenderer，物體賦值上對應的材質，並把 X_ray shader掛載，並設置shader參數
            MeshRenderer meshRen = go.AddComponent<MeshRenderer>();
            meshRen.materials = meshRender[i].materials;

            for (int s = 0; s < meshRen.materials.Length; s++)
            {
                meshRen.materials[s].shader = xRayShader;
                //meshRen.materials[s].SetFloat("_Pow", 2f);
                ShadowColor.a = 1;
                meshRen.materials[s].SetColor("_BaseColor", ShadowColor);
            }



            //meshRen.material.shader = xRayShader;
            //meshRen.material.SetFloat("_Pow", 2f);
            //shadowColor.a = 5;
            //meshRen.material.SetColor("_XRayColor", shadowColor);


            //關閉殘影效果的陰影
            meshRen.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            //設置物體的位置旋轉和比例，為對應 SkinnedMeshRenderer 物體的位置旋轉和比例
            go.transform.localScale = meshRender[i].transform.localScale * ShadowScale;
            go.transform.position = meshRender[i].transform.position;
            go.transform.rotation = meshRender[i].transform.rotation;

            //給 XRayItem  meshRenderer 參數賦值
            item.meshRenderer = meshRen;

        }
    }
    //void ShadowTrigger()
    //{
    //    shadowTrigger_ = true;
    //    //UniTask.Delay(2500);
    //    //shadowTrigger_ = false;
    //}
}