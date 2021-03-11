using UnityEngine.Audio;
using System;
using UnityEngine;

//Thanks to Brackeys for full Idea and Script (youtube.com/watch?v=6OT43pvUyfY&t=260s&ab_channel=Brackeys)
public class AudioManager : MonoBehaviour
{
    public SoundClass[] sounds;
    /// <summary>
    /// Single instance of AudioManager class
    /// </summary>
    public static AudioManager instance;

    /// <summary>
    /// Singleton implementation as well as instantiation of sounds added through Inspector(audio clip, volume).
    /// </summary>
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        foreach (SoundClass s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
        }
    }
    
    /// <summary>
    /// Method for playing different source based on name passed as parameter.
    /// Logs case if source with specified name not found.
    /// </summary>
    /// <param name="name"> Name of the source to be played.</param>
    public void Play(string name)
    {
        SoundClass s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            s.source.Play();
        }
        else
        {
            Debug.Log("Sound " + name + " not found");
        }
    }

    /// <summary>
    /// Modified Play method with functionality of stopping certain sound, on same way as Play.
    /// Logs case if source with specified name not found.
    /// </summary>
    /// <param name="name"> Name of the source playing the clip, desired to be turned off. </param>
    public void Stop(string name)
    {
        SoundClass s = Array.Find(sounds, sound => sound.Name == name);
        if (s != null)
        {
            s.source.Stop();
        }
        else
        {
            Debug.Log("Sound " + name + " not found");
        }
    }
}
