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

    #region Getters
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
