using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioClip> sounds;

    private AudioSource bgmAudio;
    private AudioSource efxAudio;
    public float allVolume { get; private set; } = 1f;
    public float efxVolume { get; private set; } = 1f;
    public float bgmVolume { get; private set; } = 1f;
    AsyncOperationHandle soundHandle;
    void Start()
    {
        if (transform.TryGetComponent(out AudioSource source))
        {
            bgmAudio = GetComponents<AudioSource>()[0];
            efxAudio = GetComponents<AudioSource>()[1];
        }
        else
        {
            bgmAudio = transform.AddComponent<AudioSource>();
            efxAudio = transform.AddComponent<AudioSource>();
        }

        bgmAudio.loop = true;
        sounds = new Dictionary<string, AudioClip>();

        SceneLoadManager.Instance.LoadScene("TaewoongTest");
    }

    public override void Release()
    {
        sounds.Clear();
        Addressables.Release(soundHandle);
        Debug.Log(7);
    }

    public void SetEFXVolume(float volume)
    {
        efxVolume = volume;
        efxAudio.volume = efxVolume * allVolume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmAudio.volume = bgmVolume * allVolume;
    }

    public void SetAllVolume(float volume)
    {
        allVolume = volume;
        SetBGMVolume(bgmVolume);
        SetBGMVolume(efxVolume);
    }

    public void InsertSound(string key, AudioClip audioClip)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            Debug.Log($"{key} is duplicate in sounds");
            return;
        }
        sounds.Add(key, clip);
    }
    public void PlayBGMSound(string key)
    {
        if (sounds == null)
        {
            Debug.LogWarning("Sounds is null");
            return;
        }

        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            bgmAudio.clip = clip;
            bgmAudio.Play();
        }
        else
        {
            Debug.LogWarning("{key} sound not assigned in SoundManager.");
        }
    }
    public void PlayEFXSound(string key)
    {
        if (sounds.TryGetValue(key, out AudioClip clip))
        {
            efxAudio.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning("{key} sound not assigned in SoundManager.");
        }
    }
    public AsyncOperationHandle LoadSound(string label)
    {
        var handle = ResourceManager.Instance.LoadResource<AudioClip>("Stage1", clip =>
        {
            sounds.Add(clip.name, clip);
        });
        handle.Completed += OnLoadCompleteObject;
        soundHandle = handle;
        return soundHandle;
    }

    private void OnLoadCompleteObject<T>(AsyncOperationHandle<IList<T>> handle) where T : UnityEngine.Object
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("Load Sounds Succeeded");
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError("Load Sounds Failed");
        }
    }
}
