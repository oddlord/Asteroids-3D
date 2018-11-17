using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton pattern
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

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
    [Header("Tags")]
    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private string asteroidTag = "Asteroid";
    [SerializeField]
    private string projectileTag = "Projectile";
    [SerializeField]
    private string playerShipTag = "PlayerShip";
    [SerializeField]
    private string playerShipBrokenTag = "PlayerShipBroken";
    [SerializeField]
    private string shipModuleTag = "ShipModule";
    #endregion

    #region Private attributes
    private GameState state;
    private GameState previousState;
    #endregion

    #region Enums
    public enum GameState
    {
        NewGame,
        Settings,
        Playing,
        GameOver
    }
    #endregion

    #region Start
    private void Start()
    {
        state = GameState.NewGame;
        EnterNewGameMenu();
    }
    #endregion

    #region State update
    private void UpdateState(GameState newState)
    {
        previousState = state;
        state = newState;
        UIManager.Instance.UpdateUI();
    }

    private void EnterNewGameMenu()
    {
        UpdateState(GameState.NewGame);
        SoundManager.Instance.PlayMenuTrack();
    }

    public void StartNewGame()
    {
        UpdateState(GameState.Playing);
        ScoreManager.Instance.Init();
        LivesManager.Instance.Init();
        AsteroidSpawner.Instance.Init();
        PlayerManager.Instance.Spawn();
        SoundManager.Instance.PlayRandomGameTrack();
    }

    public void OpenSettings()
    {
        UpdateState(GameState.Settings);
    }

    public void CloseSettings()
    {
        UpdateState(previousState);
    }

    public void EnterGameOverMenu()
    {
        UpdateState(GameState.GameOver);
        SoundManager.Instance.PlayMenuTrack();
    }
    #endregion

    #region Getters
    public GameState GetGameState()
    {
        return state;
    }

    public string GetPlayerTag()
    {
        return playerTag;
    }

    public string GetAsteroidTag()
    {
        return asteroidTag;
    }

    public string GetProjectileTag()
    {
        return projectileTag;
    }

    public string GetPlayerShipTag()
    {
        return playerShipTag;
    }

    public string GetPlayerShipBrokenTag()
    {
        return playerShipBrokenTag;
    }

    public string GetShipModuleTag()
    {
        return shipModuleTag;
    }
    #endregion
}
