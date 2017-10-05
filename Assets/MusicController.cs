using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{

    public AudioMixerGroup musicMixer;


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