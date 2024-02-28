using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostEnemyUI : MonoBehaviour
{
    [SerializeField]
    Slider greenHp_Slider_;
    [SerializeField]
    Slider redHp_Slider_;
    [SerializeField, Tooltip("EnemyBeHit Data")]
    EnemyBeHitTest enemyData_;

    // Start is called before the first frame update
    void Start()
    {
        enemyData_ = GetComponentInParent<EnemyBeHitTest>();
    }

    // Update is called once per frame
    void Update()
    {
        greenHp_Slider_.value = enemyData_.HealthPoint / 100;

        redHp_Slider_.value = Mathf.Lerp(redHp_Slider_.value, enemyData_.HealthPoint / 100, Time.deltaTime * 10);
    }
}
