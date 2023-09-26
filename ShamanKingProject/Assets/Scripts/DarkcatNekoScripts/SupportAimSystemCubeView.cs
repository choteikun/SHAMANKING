using Gamemanager;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class SupportAimSystemCubeView : MonoBehaviour
{
    [SerializeField] List<GameObject> inTriggerObjects_ = new List<GameObject>();
    GameObject nowAimingObject_;

    private void Start()
    {
        GameManager.Instance.MainGameEvent.OnAimingButtonTrigger.Where(cmd => !cmd.AimingButtonIsPressed).Subscribe(cmd => { inTriggerObjects_.Clear(); });
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Possessable") || other.CompareTag("Biteable") || other.CompareTag("Enemy"))
        {
            Debug.Log("Hit!!" + " " + other.name);
            inTriggerObjects_.Insert(0, other.gameObject);
            nowAimingObject_ = other.gameObject;
            var hitInfo = other.GetComponent<HitableItemTest>();
            GameManager.Instance.MainGameEvent.Send(new SupportAimSystemGetHitableItemCommand() { HitObject = other.gameObject, HitableItemInfo = hitInfo });//之後要注意clone問題 
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Possessable") || other.CompareTag("Biteable") || other.CompareTag("Enemy"))
        {

            Debug.Log("Out!!" + " " + other.name);
            removeInTriggerObject(other.gameObject);

        }
    }
    void removeInTriggerObject(GameObject beRemoveItem)
    {
        for (int i = 0; i < inTriggerObjects_.Count; i++)
        {
            if (inTriggerObjects_[i] == beRemoveItem)
            {
                if (i == 0)
                {
                    GameManager.Instance.MainGameEvent.Send(new SupportAimSystemLeaveHitableItemCommand() { LeaveObject = inTriggerObjects_[0].gameObject });//之後要注意clone問題
                    inTriggerObjects_.Remove(inTriggerObjects_[i]);
                    if (inTriggerObjects_.Count > 0)
                    {
                        nowAimingObject_ = inTriggerObjects_[0].gameObject;
                        var hitInfo = inTriggerObjects_[0].GetComponent<HitableItemTest>();
                        GameManager.Instance.MainGameEvent.Send(new SupportAimSystemGetHitableItemCommand() { HitObject = inTriggerObjects_[0], HitableItemInfo = hitInfo });//之後要注意clone問題 
                    }
                }
                else
                {
                    inTriggerObjects_.Remove(inTriggerObjects_[i]);
                }
            }
        }
    }
}

