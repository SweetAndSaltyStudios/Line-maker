using UnityEngine;

public class MusicPlayer : Singelton<MusicPlayer>
{
    private AudioSource audioSource;
    private AudioClip[] musicTracks;
    private int currentMusicTrackIndex;
    private bool isPaused;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        musicTracks = ResourceManager.Instance.GetFromResourcesAsArray<AudioClip>("Audio/Music");
    }

    private void Start()
    {
        SetAudioSource(audioSource, true, 0.5f);
        PlayRandomMusicTrack();
    }

    private void PlayRandomMusicTrack()
    {
        var randomMusicTrackIndex = Random.Range(0, musicTracks.Length);
        PlayMusicTrack(currentMusicTrackIndex == randomMusicTrackIndex ? currentMusicTrackIndex + 1 : randomMusicTrackIndex);
    }

    public void SetAudioSource(AudioSource audioSource, bool loop = false, float volume = 1f, float pitch = 1f)
    {
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = loop;
    }

    public void PlayMusicTrack()
    {
        PlayMusicTrack(currentMusicTrackIndex);
    }

    private void PlayMusicTrack(int musicTrackIndex)
    {
        ChangeMusicTrack(musicTrackIndex);
        audioSource.Play();
    }

    public void PlayNextMusicTrack()
    {
        ChangeMusicTrackIndex(currentMusicTrackIndex + 1);

        StopMusicTrack();

        ChangeMusicTrack(currentMusicTrackIndex);

        PlayMusicTrack(currentMusicTrackIndex);
    }

    public void PlayPreviousMusicTrack()
    {
        ChangeMusicTrackIndex(currentMusicTrackIndex - 1);

        StopMusicTrack();

        ChangeMusicTrack(currentMusicTrackIndex);

        PlayMusicTrack(currentMusicTrackIndex);
    }

    private void ChangeMusicTrackIndex(int newMusicTrackIndex)
    {
        currentMusicTrackIndex = newMusicTrackIndex;

        if (currentMusicTrackIndex > musicTracks.Length - 1)
        {
            currentMusicTrackIndex = 0;
        }
        else if(currentMusicTrackIndex <= 0)
        {
            currentMusicTrackIndex = musicTracks.Length - 1;
        }
    }

    private void ChangeMusicTrack(int musicTrackIndex)
    {
        audioSource.clip = musicTracks[currentMusicTrackIndex = musicTrackIndex];
    }

    public void StopMusicTrack()
    {
        audioSource.Stop();
    }
   
    public void PauseMusicTrack()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            audioSource.Pause();
        }
        else
        {
            audioSource.UnPause();
        }

    }

    public void MuteMusicTrack()
    {
        audioSource.mute = !audioSource.mute;
    }
}
