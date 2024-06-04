using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolumeManager : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterSlider;

    // Use this for initialization
    void Start()
    {
        masterSlider.value = -20;
    }

    public void SetMasterVolume()
    {
        mixer.SetFloat("MasterVolume", masterSlider.value);
    }
}
