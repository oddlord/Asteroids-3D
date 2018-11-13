using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region Singleton Pattern
    private static AsteroidSpawner instance;

    public static AsteroidSpawner Instance { get { return instance; } }

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

    [Header("Asteroid Prefabs")]
    [SerializeField]
    private List<GameObject> asteroidPrefabs;
    
    [Header("Asteroid Physics")]
    [SerializeField]
    private float asteroidMass = 100f;
    [SerializeField]
    private float angularVelocity = 0.25f;
    [SerializeField]
    private float movementVelocity = 5f;

    [Header("Initial Asteroids")]
    [SerializeField]
    private int minInitialAsteroids = 3;
    [SerializeField]
    private int maxInitialAsteroids = 4;

    [Header("Fragments")]
    [SerializeField]
    private float fragmentVelocityIncreaseFactor = 0.5f;
    [SerializeField]
    private float fragmentRatio = 0.5f;

    [Header("Fragments Spawning")]
    [SerializeField]
    private int minFragments = 2;
    [SerializeField]
    private int maxFragments = 2;

    [SerializeField]
    private float respawnTime = 1f;

    private int spawnedLast;

    void Start()
    {
        spawnedLast = 0;
        SpawnAsteroids(false);
    }

    private void Update()
    {
        if (transform.childCount == 0)
        {
            SpawnAsteroids(true);
        }
    }

    IEnumerator WaitForRespawn(GameObject asteroid)
    {
        yield return new WaitForSeconds(respawnTime);
        asteroid.SetActive(true);

    }

    public float GetFragmentRatio()
    {
        return fragmentRatio;
    }

    private void SpawnAsteroid(int asteroidTypeIdx, Vector3 spawnPosition, int size, float velocity, bool waitRespawnTime)
    {
        GameObject asteroid = Instantiate(asteroidPrefabs[asteroidTypeIdx], spawnPosition, Random.rotation) as GameObject;
        asteroid.transform.SetParent(transform);
        asteroid.tag = GameManager.Instance.GetAsteroidTag();

        Rigidbody asteroidRB = asteroid.AddComponent<Rigidbody>();
        asteroidRB.mass = asteroidMass;
        asteroidRB.drag = 0f;
        asteroidRB.angularDrag = 0f;
        asteroidRB.useGravity = false;
        asteroidRB.constraints = RigidbodyConstraints.FreezePositionZ;
        RandomMover randomMover = asteroid.AddComponent<RandomMover>();
        randomMover.SetVelocity(velocity);
        randomMover.SetAngularVelocity(angularVelocity);
        randomMover.GivePush();
        asteroid.AddComponent<ScreenWrapper>();
        AsteroidManager asteroidManager = asteroid.AddComponent<AsteroidManager>();
        asteroidManager.Spawn(size);
        asteroid.AddComponent<AsteroidCollider>();

        if (waitRespawnTime)
        {
            asteroid.SetActive(false);
            StartCoroutine("WaitForRespawn", asteroid);
        }
    }

    private void SpawnAsteroids(bool waitRespawnTime)
    {
        if (spawnedLast == 0)
        {
            spawnedLast = Random.Range(minInitialAsteroids, maxInitialAsteroids + 1);
        }
        else
        {
            // at each subsequent new level
            // spawn either as the previous level
            // or one more
            // to increase difficulty as the game goes on
            spawnedLast = Random.Range(this.spawnedLast, this.spawnedLast + 2);
        }
        
        for (int i = 0; i < spawnedLast; i++)
        {
            int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
            // uncomment this to have randomised asteroid sizes at the beginning of each level
            // int asteroidSize = Random.Range(1, 4);
            int asteroidSize = 3;

            // This is to spawn asteroids on the borders of the screen
            // side represent left/right or bottom/top of the non-random axis
            float side = Random.Range(0, 2);
            // randomPos is the position in the random axis
            float randomPos = Random.Range(0f, 1f);
            float randomXPos;
            float randomYPos;
            // choose randomly which side to be random and which not
            if (Random.value < 0.5f)
            {
                randomXPos = randomPos;
                randomYPos = side;
            }
            else
            {
                randomXPos = side;
                randomYPos = randomPos;
            }
            Vector3 randomPositionOnScreenBorder = Camera.main.ViewportToWorldPoint(new Vector3(randomXPos, randomYPos, -Camera.main.transform.position.z));

            SpawnAsteroid(asteroidTypeIdx, randomPositionOnScreenBorder, asteroidSize, movementVelocity, waitRespawnTime);
        }
    }

    public void SpawnFragments(Vector3 spawnPosition, int originalAsteroidSize)
    {
        if (originalAsteroidSize > 1)
        {
            int fragmentSize = originalAsteroidSize - 1;
            int fragmentCount = Random.Range(minFragments, maxFragments + 1);
            for (int i = 0; i < fragmentCount; i++)
            {
                int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
                float fragmentVelocity = movementVelocity;
                for (int j = fragmentSize; j < 3; j++)
                {
                    fragmentVelocity += fragmentVelocity * fragmentVelocityIncreaseFactor;
                }
                SpawnAsteroid(asteroidTypeIdx, spawnPosition, fragmentSize, fragmentVelocity, false);
            }
        }
        // otherwise the asteroid was already of the smallest size
        // and should generate no fragments
    }
}
