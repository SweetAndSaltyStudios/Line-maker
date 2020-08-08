using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singelton<AudioManager>
{
    private AudioMixer audioMixer;

    private AudioClip[] sfxs;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        audioMixer = ResourceManager.Instance.GetFromResources<AudioMixer>("Audio/Mixers/", "AudioMixer");
        sfxs = ResourceManager.Instance.GetFromResourcesAsArray<AudioClip>("Audio/Sfx");
    }

    public void PlayAudioClipAtPosition(string clipName, Vector2 position)
    {
        var audioSource = new GameObject(clipName + "_Sound").AddComponent<AudioSource>();
        audioSource.transform.position = position;
        audioSource.PlayOneShot(GetAudioClip(clipName, sfxs));

        Destroy(audioSource.gameObject, 1f);
    }

    private AudioClip GetAudioClip(string clipName, AudioClip[] audioSources)
    {
        foreach (var audioClip in audioSources)
        {
            if (audioClip.name.Equals(clipName))
            {
                return audioClip;
            }
        }

        Debug.LogError("There is not an audio clip by name of -- " + clipName + "--");
        return null;
    }
}
