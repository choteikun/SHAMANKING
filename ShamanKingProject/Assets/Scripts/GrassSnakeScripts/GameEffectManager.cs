using System.Collections.Generic;
using UnityEngine;

public class GameEffectManager : MonoBehaviour
{
    //特效池
    Dictionary<int, List<GameEffect>> effectPool = new Dictionary<int, List<GameEffect>>();
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 新增特效到世界
    /// </summary>
    /// <returns>The world effect.</returns>
    /// <param name="effectId">配置表中的id.</param>
    /// <param name="pos">出生位置.</param>
    /// <param name="scale">特效的尺寸.</param>

}
