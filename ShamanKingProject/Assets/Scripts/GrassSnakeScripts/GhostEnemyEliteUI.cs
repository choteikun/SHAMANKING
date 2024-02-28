using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostEnemyEliteUI : MonoBehaviour
{
    [SerializeField]
    Slider greenHp_Slider_;
    [SerializeField]
    Slider redHp_Slider_;
    [SerializeField]
    Slider greenBK_Slider_;
    [SerializeField]
    Slider redBK_Slider_;
    [SerializeField, Tooltip("EnemyBeHit Data")]
    EnemyBeHitTest enemyData_;

    // Start is called before the first frame update
    void Start()
    {
        enemyData_ ??= GetComponentInParent<EnemyBeHitTest>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = new Vector3(transform.position.x, 0, transform.position.z) - new Vector3(Camera.main.transform.position.x, 0, Camera.main.transform.position.z);

        greenHp_Slider_.value = enemyData_.HealthPoint / enemyData_.GetMaxHealthPoint();

        redHp_Slider_.value = Mathf.Lerp(redHp_Slider_.value, enemyData_.HealthPoint / enemyData_.GetMaxHealthPoint(), Time.deltaTime * 10);

        greenBK_Slider_.value = enemyData_.BreakPoint / enemyData_.MaxBreakPoint;

        redBK_Slider_.value = Mathf.Lerp(redBK_Slider_.value, enemyData_.HealthPoint / enemyData_.MaxBreakPoint, Time.deltaTime * 10);
    }
}
