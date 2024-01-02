using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Datamanager;
using UnityEngine.VFX;
using Gamemanager;

public class FlameThrowerBehavior : MonoBehaviour
{
    [SerializeField] GameObject flameThrowerManagerCenter_;
    [SerializeField] GameObject bossPos_;
    [SerializeField] GameObject flameHitboxSpawnPos_;
    [SerializeField] float centerDistance_;
    [SerializeField] bool isFlameThrowing = false;
    [SerializeField] int frameCounter = 0;
    [SerializeField] VisualEffect fireBeam_;
    [SerializeField] GameObject fireBeamObject_;
    [SerializeField] GameObject fireBeamFollowObject_;
    void Start()
    {
        GameManager.Instance.HellDogGameEvent.SetSubscribe(GameManager.Instance.HellDogGameEvent.OnBossCallFlameThrowerSwitch, cmd => { isFlameThrowing = cmd.TurnedOn; startFlameThrowerVFX(cmd); });
    }

    // Update is called once per frame
    void Update()
    {
        throwerManagerPosUpdater();
    }
    private void FixedUpdate()
    {
        hitboxSpawner();
    }
    void throwerManagerPosUpdater()
    {
        var dir = bossPos_.transform.forward.normalized;
        dir.y = 0;
        flameThrowerManagerCenter_.transform.position = bossPos_.transform.position + dir * centerDistance_;
        flameThrowerManagerCenter_.transform.rotation = bossPos_.transform.rotation;
    }
    void hitboxSpawner()
    {
        if (isFlameThrowing) 
        {
            frameCounter += 1;
            fireBeamObject_.transform.position = fireBeamFollowObject_.transform.position;
            var rotation = fireBeamFollowObject_.transform.rotation.eulerAngles;
            rotation.x = 0;
            fireBeamObject_.transform.rotation = Quaternion.Euler(rotation);
            if (frameCounter == 4)
            {
                frameCounter = 0;
                var hitboxPrefab = GameContainer.Get<DataManager>().GetDataByID<GameEffectTemplete>(21).PrefabPath;
                var hitboxObject = Instantiate(hitboxPrefab, flameHitboxSpawnPos_.transform.position, flameHitboxSpawnPos_.transform.rotation);
            }
        }
        else
        {
            frameCounter = 0;
        }
    }
    void startFlameThrowerVFX(BossCallFlameThrowerSwitchCommand cmd)
    {
        if (cmd.TurnedOn)
        {           
        fireBeam_.Play();
        }
    }
         
}
