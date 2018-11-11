using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton Pattern
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

    [SerializeField]
    Text scoreText;

    [SerializeField]
    private string playerTag = "Player";
    [SerializeField]
    private string asteroidTag = "Asteroid";
    [SerializeField]
    private string projectileTag = "Projectile";
    [SerializeField]
    private string playerShipTag = "PlayerShip";
    [SerializeField]
    private string shipModuleTag = "ShipModule";

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

    public string GetShipModuleTag()
    {
        return shipModuleTag;
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
