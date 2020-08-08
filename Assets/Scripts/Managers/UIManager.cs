using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singelton<UIManager>
{
    private Transform hudCanvas;

    public UIPanel MainMenuPanel { get; private set; }
    public UIPanel PausePanel { get; private set; }
    public UIPanel InGamePanel { get; private set; }

    [SerializeField]
    private Image energyBar;

    [SerializeField]
    private TextMeshProUGUI energyValueText;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        ChangeUILayout(GAME_STATE.IN_GAME);
    }

    private void Initialize()
    {
        hudCanvas = transform.Find("HUDCanvas");

        MainMenuPanel = hudCanvas.Find("MainMenuPanel").GetComponent<UIPanel>();
        PausePanel = hudCanvas.Find("PausePanel").GetComponent<UIPanel>();
        InGamePanel = hudCanvas.Find("InGamePanel").GetComponent<UIPanel>();
    }

    private void Update()
    {
        if (energyBar.fillAmount == LevelManager.Instance.CurrentEnergy)
            return;

        UpdateEnergyBarVisuals(LevelManager.Instance.CurrentEnergy);
    }

    private void UpdateEnergyBarVisuals(float newValue)
    {
        energyBar.fillAmount = Mathf.Lerp(energyBar.fillAmount, newValue * 0.01f, Time.deltaTime);

        energyValueText.text = newValue.ToString("00");
    }

    public void ChangeUILayout(GAME_STATE newGameState)
    {
        GameMaster.Instance.ChangeGameState(newGameState);

        switch (newGameState)
        {
            case GAME_STATE.IN_GAME:

                MainMenuPanel.gameObject.SetActive(false);
                PausePanel.gameObject.SetActive(false);
                InGamePanel.gameObject.SetActive(true);

                break;

            case GAME_STATE.PAUSED:

                MainMenuPanel.gameObject.SetActive(false);
                PausePanel.gameObject.SetActive(true);
                InGamePanel.gameObject.SetActive(true);
         
                break;

            case GAME_STATE.MAIN_MENU:

                MainMenuPanel.gameObject.SetActive(true);
                PausePanel.gameObject.SetActive(false);
                InGamePanel.gameObject.SetActive(false);

                break;

            default:

                break;
        }
    }
}
