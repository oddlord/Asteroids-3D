using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton pattern
    private static UIManager instance;

    public static UIManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    #region SerializeField attributes    
    [Header("UI Panels")]
    [SerializeField]
    private RectTransform newGamePanel;
    [SerializeField]
    private RectTransform settings;
    [SerializeField]
    private RectTransform gameUI;
    [SerializeField]
    private RectTransform gameoverPanel;

    [Header("Elements")]
    [SerializeField]
    private Text finalScoreText;
    [SerializeField]
    private Button newGameButton;
    [SerializeField]
    private Button playAgainButton;
    [SerializeField]
    private Slider musicSlider;
    [SerializeField]
    private Slider soundEffectsSlider;

    [Header("Wait To Select")]
    [SerializeField]
    private float waitToSelectElement = 0.01f;
    #endregion

    #region Private attributes
    private bool initialSFXSliderSet;
    #endregion

    #region Start
    private void Start()
    {
        initialSFXSliderSet = false;
    }
    #endregion

    #region Update UI
    public void AddVirtualJoystick(GameObject joystick)
    {
        joystick.transform.SetParent(gameUI.transform, false);
    }

    private void DisableAllUI()
    {
        newGamePanel.gameObject.SetActive(false);
        settings.gameObject.SetActive(false);
        gameUI.gameObject.SetActive(false);
        gameoverPanel.gameObject.SetActive(false);
    }

    public void UpdateUI()
    {
        DisableAllUI();
        GameManager.GameState state = GameManager.Instance.GetGameState();
        switch (state)
        {
            case GameManager.GameState.NewGame:
                newGamePanel.gameObject.SetActive(true);
                NewGameMenu();
                break;
            case GameManager.GameState.Settings:
                settings.gameObject.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                gameUI.gameObject.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                gameoverPanel.gameObject.SetActive(true);
                GameOverMenu();
                break;
            default:
                break;
        }
    }

    private void NewGameMenu()
    {
        if (!InputManager.Instance.IsMobile())
        {
            StartCoroutine(WaitToSelectButton(newGameButton));
        }
    }

    public void StartNewGame()
    {
        GameManager.Instance.StartNewGame();
    }

    public void OpenSettings()
    {
        GameManager.Instance.OpenSettings();
        musicSlider.value = SoundManager.Instance.GetMusicVolume();
        initialSFXSliderSet = true;
        soundEffectsSlider.value = SoundManager.Instance.GetSFXVolume();
        if (!InputManager.Instance.IsMobile())
        {
            StartCoroutine(WaitToSelectSlider(musicSlider));
        }
    }

    public void SaveSettings()
    {
        SoundManager.Instance.SaveVolumes();
        GameManager.Instance.CloseSettings();
    }

    public void CancelSettings()
    {
        SoundManager.Instance.RollbackVolumes();
        GameManager.Instance.CloseSettings();
    }

    public void GameOverMenu()
    {
        finalScoreText.text = "Score: " + ScoreManager.Instance.GetScore();
        if (!InputManager.Instance.IsMobile())
        {
            StartCoroutine(WaitToSelectButton(playAgainButton));
        }
    }

    private IEnumerator WaitToSelectButton(Button button)
    {
        yield return new WaitForSeconds(waitToSelectElement);
        button.Select();
    }

    private IEnumerator WaitToSelectSlider(Slider slider)
    {
        yield return new WaitForSeconds(waitToSelectElement);
        slider.Select();
    }
    #endregion

    #region Getters
    public bool IsInitialSFXSLiderSet()
    {
        return initialSFXSliderSet;
    }
    #endregion

    #region Setters
    public void DisableInitialSFXSLiderSet()
    {
        initialSFXSliderSet = false;
    }
    #endregion
}
