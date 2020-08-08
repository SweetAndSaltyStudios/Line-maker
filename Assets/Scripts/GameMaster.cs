using UnityEngine;

public enum GAME_STATE
{
    IN_GAME,
    PAUSED,
    MAIN_MENU
}

public class GameMaster : Singelton<GameMaster>
{
    public GAME_STATE CurrentGameState
    {
        get;
        private set;
    }

    public void ChangeGameState(GAME_STATE newGameState)
    {
        CurrentGameState = newGameState;

        switch (CurrentGameState)
        {
            case GAME_STATE.IN_GAME:

                break;

            case GAME_STATE.PAUSED:

                break;

            case GAME_STATE.MAIN_MENU:

                break;

            default:

                break;
        }
    }
}
