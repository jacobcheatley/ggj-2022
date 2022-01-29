using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private AudioMixerSnapshot[] snapshots;

    [SerializeField]
    private AudioSource sfxSource;

    public static SoundManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void TransitionMusic(int snapshotIndex, float timeToReach)
    {
        instance.audioMixer.TransitionToSnapshots(new AudioMixerSnapshot[] { instance.snapshots[snapshotIndex] }, new float[] { 1 }, timeToReach);
    }

    public void PlayClip(AudioClip clip, float volume = 1f)
    {
        instance.sfxSource.PlayOneShot(clip, volume);
    }
}
