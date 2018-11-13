using System.Collections.Generic;
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

    [Header("UI")]
    [SerializeField]
    private GameObject joystick;
    [SerializeField]
    private Text scoreText;
    
    [Header("Player")]
    [SerializeField]
    private Transform player;

    [Header("Scores")]
    [SerializeField]
    private int largeAsteroidScore = 20;
    [SerializeField]
    private int mediumAsteroidScore = 50;
    [SerializeField]
    private int smallAsteroidScore = 100;
    [SerializeField]
    private int pointsForNewLife = 10000;

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
    private string shipModuleTag = "ShipModule";

    [Header("Device")]
    [SerializeField]
    private Device device;

    public enum Device
    {
        Mobile,
        PC
    }

    private void Start()
    {
        if (device == Device.Mobile)
        {
            joystick.SetActive(true);
        }
    }

    public FixedJoystick GetJoystick()
    {
        return joystick.GetComponent<FixedJoystick>();
    }

    public void SpawnPlayer()
    {
        for (int i = 0; i < player.childCount; i++)
        {
            GameObject child = player.GetChild(i).gameObject;
            Destroy(child);
        }
        
        PlayerManager playerManager = player.GetComponent<PlayerManager>();

        GameObject playerShipPrefab = playerManager.GetPlayerShipPrefab();
        GameObject playerShip = Instantiate(playerShipPrefab, playerShipPrefab.transform.position, playerShipPrefab.transform.rotation) as GameObject;
        playerShip.transform.SetParent(player, false);

        playerManager.SetPlayerShip(playerShip.transform);

        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z));
        player.position = screenCenter;
    }

    public int GetAsteroidScore(int size)
    {
        int score = 0;
        if (size == 3)
        {
            score = largeAsteroidScore;
        }
        else if (size == 2)
        {
            score = mediumAsteroidScore;
        }
        else if (size == 1)
        {
            score = smallAsteroidScore;
        } else if (size > 3)
        {
            // just in case
            score = size * 100;
        }
        return score;
    }

    public int GetPointsForNewLife()
    {
        return pointsForNewLife;
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

    public string GetShipModuleTag()
    {
        return shipModuleTag;
    }

    public Device GetDevice()
    {
        return device;
    }

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
