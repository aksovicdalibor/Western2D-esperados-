using UnityEngine.Audio;
using UnityEngine;

//Again, Brackeys rocked! (youtube.com/watch?v=6OT43pvUyfY&t=260s&ab_channel=Brackeys)

/// <summary>
/// Custom class made for easily adding clips in AudioManager through Inspector.
/// </summary>
[System.Serializable]
public class SoundClass
{
    /// <summary>
    /// Name of the clip.
    /// </summary>
    public string Name;

    /// <summary>
    /// Clip refferenced through Inspector
    /// </summary>
    public AudioClip clip;

    /// <summary>
    /// Starting volume of the clip. Scrollable.
    /// </summary>
    [Range(0,1)]
    public float volume;

    /// <summary>
    /// Audio source instatiated through AudioManager script.
    /// </summary>
    [HideInInspector]
    public AudioSource source;
}
