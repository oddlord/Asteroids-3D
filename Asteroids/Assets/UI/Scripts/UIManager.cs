using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LifeIconPool))]
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

    [Header("Wait To Select")]
    [SerializeField]
    private float waitToSelectButton = 0.01f;
    #endregion

    #region Private attributes
    private LifeIconPool lifeIconPool;
    #endregion

    #region Start
    private void Start()
    {
        lifeIconPool = GetComponent<LifeIconPool>();
        lifeIconPool.InitPool();
    }
    #endregion

    #region Add Virtual Joystick
    public void AddVirtualJoystick(GameObject joystick)
    {
        joystick.transform.SetParent(gameUI.transform, false);
    }
    #endregion

    #region Update UI
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
                break;
            case GameManager.GameState.Settings:
                settings.gameObject.SetActive(true);
                break;
            case GameManager.GameState.Playing:
                gameUI.gameObject.SetActive(true);
                break;
            case GameManager.GameState.GameOver:
                gameoverPanel.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void NewGame()
    {
        GameManager.Instance.StartNewGame();
        if (!InputManager.Instance.IsMobile())
        {
            StartCoroutine(WaitToSelectButton(newGameButton));
        }
    }

    public void GameOver(int score)
    {
        finalScoreText.text = "Score: " + score.ToString();
        if (!InputManager.Instance.IsMobile())
        {
            StartCoroutine(WaitToSelectButton(playAgainButton));
        }
    }

    private IEnumerator WaitToSelectButton(Button button)
    {
        yield return new WaitForSeconds(waitToSelectButton);
        button.Select();
    }
    #endregion

    #region Getters
    public LifeIconPool GetLifeIconPool()
    {
        return lifeIconPool;
    }
    #endregion
}
