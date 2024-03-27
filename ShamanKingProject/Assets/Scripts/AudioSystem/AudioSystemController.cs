using Datamanager;
using Gamemanager;
using UnityEngine;

public class AudioSystemController : MonoBehaviour
{
    [SerializeField] GameObject audioObjectPrefab_;
    void Start()
    {
        GameManager.Instance.MainGameEvent.SetSubscribe(GameManager.Instance.MainGameEvent.OnGameCallSoundEffect, cmd => { spawnSound(cmd); });
        AudioListener.volume = 0.5f;
    }

    void spawnSound(GameCallSoundEffectGenerate cmd)
    {
        var audioObject = Instantiate(audioObjectPrefab_);
        var audioSource = audioObject.GetComponent<AudioSource>();
        var clip = GameContainer.Get<DataManager>().GetDataByID<SoundEffectDatabaseTemplete>(cmd.SoundEffectID).SoundEffect;
        audioSource.PlayOneShot(clip);
    }
}
