using Gamemanager;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class HitableItemTest : MonoBehaviour
{
    [field:SerializeField] public GameObject onHitPoint_ { get; private set; }
    [field: SerializeField] public HitObjecctTag HitTag { get; private set; }

    private void Start()
    {
        GameManager.Instance.MainGameEvent.OnPlayerBiteFinish.Where(cmd => cmd.HitObject == this.gameObject).Subscribe(cmd => { this.gameObject.SetActive(false); });
    }
}
