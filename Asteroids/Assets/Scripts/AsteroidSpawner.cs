using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    #region Singleton pattern
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

    #region SerializeField attributes
    [Header("Asteroid Prefabs")]
    [SerializeField]
    private List<GameObject> asteroidPrefabs;
    
    [Header("Asteroid Physics")]
    [SerializeField]
    private float pushForce = 50000f;
    [SerializeField]
    private float angularVelocity = 0.25f;

    [Header("Asteroid Spawning")]
    [SerializeField]
    private AsteroidData asteroidToSpawn;
    [SerializeField]
    private int minInitialAsteroids = 3;
    [SerializeField]
    private int maxInitialAsteroids = 4;
    [SerializeField]
    private float respawnTime = 1f;
    #endregion

    #region Private attributes
    private int spawnedLastCount;
    private bool spawning;
    #endregion

    #region Start and Update
    void Start()
    {
        spawnedLastCount = 0;
        spawning = false;
        SpawnAsteroids();
    }

    private void Update()
    {
        if (!spawning && transform.childCount == 0)
        {
            spawning = true;
            StartCoroutine("WaitToRespawn");
        }
    }
    #endregion

    #region Asteroid spawning
    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnAsteroids();
        spawning = false;
    }

    private void SpawnAsteroid(int asteroidTypeIdx, Vector3 spawnPosition, AsteroidData asteroidData)
    {
        GameObject asteroid = Instantiate(asteroidPrefabs[asteroidTypeIdx], spawnPosition, Random.rotation) as GameObject;
        asteroid.transform.SetParent(transform);
        asteroid.tag = GameManager.Instance.GetAsteroidTag();

        Rigidbody asteroidRB = asteroid.AddComponent<Rigidbody>();
        asteroidRB.mass = asteroidData.GetMass();
        asteroidRB.drag = 0f;
        asteroidRB.angularDrag = 0f;
        asteroidRB.useGravity = false;
        asteroidRB.constraints = RigidbodyConstraints.FreezePositionZ;
        RandomSpacePusher randomPusher = asteroid.AddComponent<RandomSpacePusher>();
        randomPusher.SetRandomPush(pushForce, angularVelocity);
        randomPusher.GivePush();
        asteroid.AddComponent<ScreenWrapper>();
        AsteroidController asteroidController = asteroid.AddComponent<AsteroidController>();
        asteroidController.SetAsteroidData(asteroidData);
        asteroid.AddComponent<AsteroidCollider>();
    }

    private void SpawnAsteroids()
    {
        if (spawnedLastCount == 0)
        {
            spawnedLastCount = Random.Range(minInitialAsteroids, maxInitialAsteroids + 1);
        }
        else
        {
            // at each new level
            // spawn either as in the previous level
            // or one more
            // to increase difficulty as the game goes on
            spawnedLastCount = Random.Range(this.spawnedLastCount, this.spawnedLastCount + 2);
        }
        
        for (int i = 0; i < spawnedLastCount; i++)
        {
            int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);

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
            Vector3 randomPositionOnScreenBorder = Camera.main.ViewportToWorldPoint(new Vector3(randomXPos, randomYPos, Mathf.Abs(transform.position.z - Camera.main.transform.position.z)));

            SpawnAsteroid(asteroidTypeIdx, randomPositionOnScreenBorder, asteroidToSpawn);
        }
    }

    public void SpawnFragments(Vector3 spawnPosition, AsteroidData asteroidData)
    {
        int fragmentCount = asteroidData.GetNumberOfFragments();
        if (fragmentCount > 0)
        {
            AsteroidData fragmentData = asteroidData.GetFragmentToSpawn();
            for (int i = 0; i < fragmentCount; i++)
            {
                int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
                SpawnAsteroid(asteroidTypeIdx, spawnPosition, fragmentData);
            }
        }
        // otherwise the asteroid was already of the smallest size
        // and should generate no fragments
    }
    #endregion
}
