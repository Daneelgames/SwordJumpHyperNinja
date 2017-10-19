using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{

    public AudioSource au;
    public AudioMixerGroup musicMixer;
    public List<AudioClip> music = new List<AudioClip>();


    public void SetMusic(int index)
    {
        au.Stop();
        au.clip = music[index];
        au.Play();
    }

    public void SlowMo(bool active)
    {
        /*
		if (active)
            musicMixer.audioMixer.SetFloat("musicFrequencyHigh", 0.25f);
        else
            musicMixer.audioMixer.SetFloat("musicFrequencyHigh", 1);
		*/
    }
}