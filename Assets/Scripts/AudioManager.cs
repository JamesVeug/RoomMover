using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum AudioClipType
{
    None = 0,
    ButtonPress,
    SlidingDoorMove,
    SlidingDoorStop,
    CameraMove,
    LightOff,
    LightOn,
    LadderBreak,
    ElevatorMoving,
    LeverPullDown,
    LeverPullUp,
    LeverMoving,
}

public class AudioManager : Singleton<AudioManager>
{
    [System.Serializable]
    public class AudioClipGroup
    {
        public AudioClipType type;
        public AudioClip clip;
        public AudioMixerGroup mixer;
    }

    public List<AudioClipGroup> AudioClips = null;

    private List<AudioSource> audioSources = new List<AudioSource>();


    public AudioSource PlaySound(AudioClipType type, bool loop = false, AudioSource source = null)
    {
        if(type == AudioClipType.None)
        {
            return null;
        }

        var group = Find(type);
        if(group == null)
        {
            Debug.Log("Can't find sound type: " + type.ToString());
            return null;
        }

        if (source == null)
        {
            source = GetAudioSource();
        }

        source.outputAudioMixerGroup = group.mixer;
        source.loop = loop;
        source.PlayOneShot(group.clip);


        return source;
    }

    public AudioSource GetAudioSource()
    {
        for (int i = 0; i < audioSources.Count; i++)
        {
            if (!audioSources[i].isPlaying)
            {
                return audioSources[i];
            }
        }

        return null;
    }

    public AudioClipGroup Find(AudioClipType type)
    {
        for (int i = 0; i < AudioClips.Count; i++)
        {
            if(AudioClips[i].type == type)
            {
                return AudioClips[i];
            }
        }

        return null;
    }
}
