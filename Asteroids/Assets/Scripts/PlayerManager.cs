using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton pattern
    private static PlayerManager instance;

    public static PlayerManager Instance { get { return instance; } }

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
    [Header("Respawn")]
    [SerializeField]
    private float respawnTime = 2f;
    [SerializeField]
    private float spawnInvulnerabilityTime = 2f;
    [SerializeField]
    private float invulnerabilityBlinkInterval = 0.1f;

    [Header("Ship Explosion")]
    [SerializeField]
    private float fragmentsPushForce = 100f;
    [SerializeField]
    private float fragmentsAngularVelocity = 2f;
    [SerializeField]
    private float momentumDampeningFactor = 0.5f;
    #endregion

    #region Private attributes
    GameObject playerShip;
    GameObject playerShipBroken;

    Vector3[] fragmentPositions;

    private bool dead;
    private bool spawning;
    #endregion

    #region Start
    void Start()
    {
        // retrieving the playerShip and playerShipBroken objects
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetPlayerShipTag())
            {
                playerShip = child;
            }
            else if (child.tag == GameManager.Instance.GetPlayerShipBrokenTag())
            {
                playerShipBroken = child;
            }
        }

        // storing the original fragment local position
        fragmentPositions = new Vector3[playerShipBroken.transform.childCount];
        for (int i = 0; i < playerShipBroken.transform.childCount; i++)
        {
            GameObject child = playerShipBroken.transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetShipModuleTag())
            {
                RandomSpacePusher randomPusher = child.GetComponent<RandomSpacePusher>();
                randomPusher.SetRandomPush(fragmentsPushForce, fragmentsAngularVelocity);

                fragmentPositions[i] = child.transform.localPosition;
            }
        }

        Spawn();
    }
    #endregion
    
    #region Getters
    public bool IsDead()
    {
        return dead;
    }

    public bool IsSpawning()
    {
        return spawning;
    }
    #endregion

    #region Spawning
    private void Spawn()
    {
        Vector3 screenCenter = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, -Camera.main.transform.position.z));
        playerShip.transform.position = screenCenter;
        playerShip.SetActive(true);
        playerShipBroken.SetActive(false);

        dead = false;
        spawning = true;
        StartCoroutine("SpawnBlink");
    }

    IEnumerator SpawnBlink()
    {
        Renderer rend = playerShip.GetComponent<Renderer>();
        float endTime = Time.time + spawnInvulnerabilityTime;
        while (Time.time < endTime)
        {
            rend.enabled = false;
            yield return new WaitForSeconds(invulnerabilityBlinkInterval);
            rend.enabled = true;
            yield return new WaitForSeconds(invulnerabilityBlinkInterval);
        }
        spawning = false;
    }
    #endregion

    #region Death
    public void Die()
    {
        Rigidbody playerShipRB = playerShip.GetComponent<Rigidbody>();
        Vector3 baseVelocity = playerShipRB.velocity;
        Vector3 shipPosition = playerShip.transform.position;
        Quaternion shipRotation = playerShip.transform.rotation;

        playerShip.GetComponent<PlayerController>().StopMovement();
        playerShip.SetActive(false);

        playerShipBroken.transform.position = shipPosition;
        playerShipBroken.transform.rotation = shipRotation;
        playerShipBroken.SetActive(true);

        for (int i = 0; i < playerShipBroken.transform.childCount; i++)
        {
            GameObject child = playerShipBroken.transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetShipModuleTag())
            {
                RandomSpacePusher randomPusher = child.GetComponent<RandomSpacePusher>();
                randomPusher.SetMomentum(baseVelocity, momentumDampeningFactor);
                randomPusher.GivePush();

                child.transform.localPosition = fragmentPositions[i];
            }
        }

        dead = true;
        LivesManager.Instance.RemoveLife();
        if (!LivesManager.Instance.IsGameover())
        {
            StartCoroutine("WaitToRespawn");
        }
    }

    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(respawnTime);
        Spawn();
    }
    #endregion
}
