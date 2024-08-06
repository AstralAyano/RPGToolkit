using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    private List<AudioSource> audioSources;
    private List<AudioClip> audioClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSources = new List<AudioSource>(GetComponentsInChildren<AudioSource>());
        audioClips = new List<AudioClip>(Resources.LoadAll<AudioClip>("Audio"));
    }

    public void PlayAudioClip(AudioClip clip, bool hasLoop)
    {
        AudioSource source = audioSources.Find(audioSource => !audioSource.isPlaying);

        if (source != null)
        {
            source.clip = clip;
            source.loop = hasLoop;
            source.Play();
        }
        else
        {
            Debug.LogWarning("No available AudioSource to play the clip.");
        }
    }

    public void PlayAudio(string name, bool hasLoop)
    {
        // Find the audio clip by name
        AudioClip clip = audioClips.Find(audioClip => audioClip.name == name);
        
        if (clip != null)
        {
            // Find an inactive AudioSource to play the clip
            AudioSource source = audioSources.Find(audioSource => !audioSource.isPlaying);
            
            if (source != null)
            {
                source.clip = clip;
                source.loop = hasLoop;
                source.Play();
            }
            else
            {
                Debug.LogWarning("No available AudioSource to play the clip.");
            }
        }
        else
        {
            Debug.LogWarning("Audio clip not found : " + name);
        }
    }

    public void StopAudio(string name)
    {
        // Find the audio clip by name
        AudioClip clip = audioClips.Find(audioClip => audioClip.name == name);
        
        if (clip != null)
        {
            // Find the AudioSource that's playing the specified clip
            AudioSource source = audioSources.Find(audioSource => audioSource.isPlaying && audioSource.clip == clip);
            
            if (source != null)
            {
                source.Stop();
            }
            else
            {
                Debug.LogWarning("No AudioSource is playing the clip : " + name);
            }
        }
        else
        {
            Debug.LogWarning("Audio clip not found : " + name);
        }
    }

    public void StopAllAudio()
    {
        foreach (AudioSource source in audioSources)
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }
}
