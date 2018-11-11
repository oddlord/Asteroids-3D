using System.Collections;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerShipPrefab;
    [SerializeField]
    private RectTransform livesCount;
    [SerializeField]
    private GameObject lifeIconPrefab;
    [SerializeField]
    private int initialLives = 3;
    [SerializeField]
    private float respawnTime = 2f;
    [SerializeField]
    private float spawnInvulnerabilityTime = 2f;
    [SerializeField]
    private float invulnerabilityBlinkInterval = 0.1f;
    [SerializeField]
    private float explodeTumble = 2f;
    [SerializeField]
    private float explodeForce = 20f;

    Transform playerShip;

    private int score;

    private int lives;

    private bool dead;
    private float lastDied;

    private bool spawned;
    private float lastSpawned;
    
    void Start()
    {
        score = 0;

        lives = initialLives;
        UpdateLivesCount();

        dead = false;
        lastDied = Time.time;

        Spawn();
    }

    public GameObject GetPlayerShipPrefab()
    {
        return playerShipPrefab;
    }

    public void SetPlayerShip(Transform ps)
    {
        playerShip = ps;
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
        GameManager.Instance.SpawnPlayer();
        dead = false;
        spawned = true;
        lastSpawned = Time.time;
        CallBlinkCoroutine();
    }

    private void SpawnEnd()
    {
        spawned = false;
    }

    private void CallBlinkCoroutine()
    {
        Renderer[] renderers = playerShip.GetComponentsInChildren<Renderer>();
        foreach (Renderer rend in renderers)
        {
            StartCoroutine("Blink", rend);
        }
    }

    IEnumerator Blink(Renderer rend)
    {
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

    public void Die()
    {
        Rigidbody playerShipRB = playerShip.GetComponent<Rigidbody>();

        for (int i = 0; i < playerShip.childCount; i++)
        {
            GameObject child = playerShip.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetShipModuleTag())
            {
                Rigidbody childRB = child.AddComponent<Rigidbody>();
                childRB.useGravity = false;
                childRB.drag = 0f;
                childRB.constraints = RigidbodyConstraints.FreezePositionZ;
                RandomMover rm = child.AddComponent<RandomMover>();
                rm.SetTumble(explodeTumble);
                rm.SetVelocity(explodeForce);
                rm.SetBaseVelocity(playerShipRB.velocity);
                child.AddComponent<ScreenWrapper>();
            }
        }

        playerShip.GetComponent<PlayerController>().StopMovement();

        dead = true;
        lastDied = Time.time;

        lives--;
        UpdateLivesCount();
    }

    public void UpdateScore(int size)
    {
        score += GameManager.Instance.GetAsteroidScore(size);
        GameManager.Instance.UpdateScore(score);
    }
}
