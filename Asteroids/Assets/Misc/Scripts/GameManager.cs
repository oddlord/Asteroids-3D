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
    [Header("UI")]
    [SerializeField]
    private GameObject joystick;
    
    [Header("Player")]
    [SerializeField]
    private Transform player;

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

    [Header("Device")]
    [SerializeField]
    private Device device;
    #endregion

    #region Enums
    public enum Device
    {
        Mobile,
        PC
    }
    #endregion

    #region Start
    private void Start()
    {
        if (device == Device.Mobile)
        {
            joystick.SetActive(true);
        }
    }
    #endregion

    #region Getters
    public SimpleTouchController GetJoystick()
    {
        return joystick.GetComponent<SimpleTouchController>();
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

    public Device GetDevice()
    {
        return device;
    }
    #endregion
}
