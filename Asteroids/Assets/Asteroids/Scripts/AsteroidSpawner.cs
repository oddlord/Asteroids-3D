using System.Collections;
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
    [Header("Asteroid Physics")]
    [SerializeField]
    private float pushForce = 50000f;
    [SerializeField]
    private float angularVelocity = 0.25f;

    [Header("Asteroid Spawning")]
    [SerializeField]
    private AsteroidData asteroidToSpawn;
    [SerializeField]
    private int minInitialAsteroids = 2;
    [SerializeField]
    private int maxInitialAsteroids = 3;
    [SerializeField]
    private float respawnTime = 1f;
    #endregion

    #region Private attributes
    private int spawnedLastCount;
    #endregion

    #region init
    public void Init()
    {
        // deactivate any active asteroid
        // from the previous playthrough
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.tag == GameManager.Instance.GetAsteroidTag() && child.activeInHierarchy)
            {
                PoolsManager.Instance.GetAsteroidsPool().SetAvailable(child);
            }
        }

        spawnedLastCount = 0;
        SpawnAsteroids();
    }
    #endregion

    #region Asteroid spawning
    IEnumerator WaitToRespawn()
    {
        yield return new WaitForSeconds(respawnTime);
        SpawnAsteroids();
    }

    private void SpawnAsteroid(Vector3 spawnPosition, AsteroidData asteroidData)
    {
        GameObject asteroid = PoolsManager.Instance.GetAsteroidsPool().GetAvailable();
        asteroid.transform.position = spawnPosition;
        asteroid.transform.rotation = Random.rotation;

        asteroid.SetActive(true);

        Rigidbody asteroidRB = asteroid.GetComponent<Rigidbody>();
        asteroidRB.mass = asteroidData.GetMass();
        RandomSpacePusher randomPusher = asteroid.GetComponent<RandomSpacePusher>();
        randomPusher.SetRandomPush(pushForce, angularVelocity);
        randomPusher.GivePush();
        AsteroidController asteroidController = asteroid.GetComponent<AsteroidController>();
        asteroidController.Init(asteroidData);
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

            SpawnAsteroid(randomPositionOnScreenBorder, asteroidToSpawn);
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
                SpawnAsteroid(spawnPosition, fragmentData);
            }
        }
        else
        {
            int activeAsteroids = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                GameObject child = transform.GetChild(i).gameObject;
                if (child.tag == GameManager.Instance.GetAsteroidTag() && child.activeInHierarchy)
                {
                    activeAsteroids++;
                    if (activeAsteroids > 1)
                    {
                        break;
                    }
                }
            }

            // only the asteroid being destroyed is active
            if (activeAsteroids == 1)
            {
                StartCoroutine(WaitToRespawn());
            }
        }
    }
    #endregion
}
