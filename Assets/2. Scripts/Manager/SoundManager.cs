using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioClip> sounds;

    private AudioSource bgmAudio;
    private AudioSource efxAudio;

    public float efxVolume { get; private set; } = 1f;
    public float bgmVolume { get; private set; } = 1f;

    void Start()
    {
        bgmAudio = GetComponents<AudioSource>()[0];
        bgmAudio.loop = true;
        efxAudio = GetComponents<AudioSource>()[1];
        sounds = new Dictionary<string, AudioClip>();
    }

    public override void Release()
    {
        sounds.Clear();
    }

    public void SetEFXVolume(float volume)
    {
        efxVolume = volume;
        efxAudio.volume = volume;
    }

    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        bgmAudio.volume = volume;
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

    }
    public void PlayEFXSound(string key)
    {

    }
}
