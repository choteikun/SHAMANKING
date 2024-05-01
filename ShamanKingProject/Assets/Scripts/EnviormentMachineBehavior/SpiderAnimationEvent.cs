using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAnimationEvent : MonoBehaviour
{
    [SerializeField] GameObject item_;
    public void activateItem()
    {
        item_.SetActive(true);
    }
}
