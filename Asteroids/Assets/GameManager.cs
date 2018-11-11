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

    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Transform player;

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
        playerShip.GetComponent<PlayerController>().SetPlayerManager(playerManager);
        playerShip.GetComponent<PlayerCollider>().SetPlayerManager(playerManager);

        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z));
        player.position = screenCenter;
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

    public void UpdateScore(int newScore)
    {
        scoreText.text = newScore.ToString();
    }
}
