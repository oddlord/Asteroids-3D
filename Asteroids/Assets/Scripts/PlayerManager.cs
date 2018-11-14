using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("Lives")]
    [SerializeField]
    private RectTransform livesCount;
    [SerializeField]
    private GameObject lifeIconPrefab;
    [SerializeField]
    private int initialLives = 3;

    [Header("Respawn")]
    [SerializeField]
    private float respawnTime = 2f;
    [SerializeField]
    private float spawnInvulnerabilityTime = 2f;
    [SerializeField]
    private float invulnerabilityBlinkInterval = 0.1f;

    [Header("Ship Explosion")]
    [SerializeField]
    private float fragmentsAngularVelocity = 2f;
    [SerializeField]
    private float fragmentsMovementVelocity = 5f;

    GameObject playerShip;
    GameObject playerShipBroken;

    Vector3[] fragmentPositions;

    private int score;

    private int lives;

    private bool dead;
    private float lastDied;

    private bool spawned;
    private float lastSpawned;
    
    void Start()
    {
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

        fragmentPositions = new Vector3[playerShipBroken.transform.childCount];
        for (int i = 0; i < playerShipBroken.transform.childCount; i++)
        {
            GameObject child = playerShipBroken.transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetShipModuleTag())
            {
                RandomSpacePusher randomPusher = child.GetComponent<RandomSpacePusher>();
                randomPusher.SetVelocity(fragmentsMovementVelocity);
                randomPusher.SetAngularVelocity(fragmentsAngularVelocity);

                fragmentPositions[i] = child.transform.localPosition;
            }
        }

        score = 0;

        lives = initialLives;
        UpdateLivesCount();

        dead = false;
        lastDied = Time.time;

        Spawn();
    }

    private float GetDiedDeltaTime()
    {
        return (Time.time - lastDied);
    }

    private float GetSpawnedDeltaTime()
    {
        return (Time.time - lastSpawned);
    }

    private void Spawn()
    {
        GameManager.Instance.SpawnPlayer(playerShip);
        playerShip.SetActive(true);
        playerShipBroken.SetActive(false);

        dead = false;
        spawned = true;
        lastSpawned = Time.time;
        StartCoroutine("Blink");
    }

    private void SpawnEnd()
    {
        spawned = false;
    }

    IEnumerator Blink()
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
    }

    private void Update()
    {
        if (dead && lives > 0 && GetDiedDeltaTime() >= respawnTime)
        {
            Spawn();
        }

        if (spawned && GetSpawnedDeltaTime() >= spawnInvulnerabilityTime)
        {
            SpawnEnd();
        }
    }

    public bool IsDead()
    {
        return dead;
    }

    public bool IsSpawned()
    {
        return spawned;
    }

    private void UpdateLivesCount()
    {
        for (int i = 0; i < livesCount.childCount; i++)
        {
            GameObject child = livesCount.GetChild(i).gameObject;
            Destroy(child);
        }

        for (int i = 0; i < lives; i++)
        {
            GameObject lifeIcon = Instantiate(lifeIconPrefab, lifeIconPrefab.transform.position, lifeIconPrefab.transform.rotation) as GameObject;
            lifeIcon.transform.SetParent(livesCount, false);
        }
    }

    private void RemoveLife()
    {
        lives--;
        UpdateLivesCount();
    }

    private void AddLife()
    {
        lives++;
        UpdateLivesCount();
    }

    public void Die()
    {
        Rigidbody playerShipRB = playerShip.GetComponent<Rigidbody>();
        Vector3 baseVelocity = playerShipRB.velocity;
        Vector3 shipPosition = playerShip.transform.position;
        Quaternion shipRotation = playerShip.transform.rotation;

        playerShip.GetComponent<PlayerController>().StopMovement();
        playerShip.SetActive(false);

        playerShipBroken.SetActive(true);
        playerShipBroken.transform.position = shipPosition;
        playerShipBroken.transform.rotation = shipRotation;

        for (int i = 0; i < playerShipBroken.transform.childCount; i++)
        {
            GameObject child = playerShipBroken.transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetShipModuleTag())
            {
                RandomSpacePusher randomPusher = child.GetComponent<RandomSpacePusher>();
                randomPusher.SetBaseVelocity(baseVelocity);
                randomPusher.GivePush();

                child.transform.localPosition = fragmentPositions[i];
            }
        }

        dead = true;
        lastDied = Time.time;

        RemoveLife();
    }

    public void UpdateScore(int size)
    {
        int oldScore = score;
        score += GameManager.Instance.GetAsteroidScore(size);

        int pointsForNewLife = GameManager.Instance.GetPointsForNewLife();
        if ((score / pointsForNewLife) > (oldScore / pointsForNewLife))
        {
            AddLife();
        }

        GameManager.Instance.UpdateScore(score);
    }
}
