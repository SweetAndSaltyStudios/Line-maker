using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventManager : Singelton<EventManager>
{
    private EventSystem eventSystem;
    private Dictionary<string, Action> buttonActions = new Dictionary<string, Action>();

    public bool IsPointerOverGameObject
    {
        get
        {
            return eventSystem.IsPointerOverGameObject();
        }
    }

    private void Awake()
    {
        Initialize();
    } 

    private void Initialize()
    {
        eventSystem = EventSystem.current;

        StoreButtonActions();
    }

    private void StoreButtonActions()
    {
        buttonActions.Add("PreviousMusicTrackButton", PreviousMusicTrackButton);
        buttonActions.Add("NextMusicTrackButton", NextMusicTrackButton);
        buttonActions.Add("StopMusicButton", StopMusicButton);
        buttonActions.Add("PauseMusicButton", PauseMusicButton);
        buttonActions.Add("PlayMusicButton", PlayMusicButton);
        buttonActions.Add("MuteMusicTrackButton", MuteMusicTrackButton);

        buttonActions.Add("PauseButton", PauseButton);
    }

    public Action GetButtonEvent(string buttonEventName)
    {
        if (buttonActions.TryGetValue(buttonEventName, out Action buttonAction))
        {
            return buttonAction;
        }

        return null;
    }

    #region BUTTON_EVENTS

    #region MUSIC_PLAYER_BUTTONS

    private void PreviousMusicTrackButton()
    {
        MusicPlayer.Instance.PlayPreviousMusicTrack();
    }

    private void NextMusicTrackButton()
    {
        MusicPlayer.Instance.PlayNextMusicTrack();
    }

    private void StopMusicButton()
    {
        MusicPlayer.Instance.StopMusicTrack();
    }

    private void PauseMusicButton()
    {
        MusicPlayer.Instance.PauseMusicTrack();
    }

    private void PlayMusicButton()
    {
        MusicPlayer.Instance.PlayMusicTrack();
    }

    private void MuteMusicTrackButton()
    {
        MusicPlayer.Instance.MuteMusicTrack();
    }

    #endregion MUSIC_PLAYER_BUTTONS

    #region IN_GAME_PANEL

    private void PauseButton()
    {
        LevelManager.Instance.PauseGame();
    }

    #endregion IN_GAME_PANEL

    #endregion BUTTON_EVENTS
}
