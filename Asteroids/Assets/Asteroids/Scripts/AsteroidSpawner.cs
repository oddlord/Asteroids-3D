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

    [SerializeField]
    private List<GameObject> asteroidPrefabs;
    [SerializeField]
    private float randomRotationTumble = 0.25f;
    [SerializeField]
    private float randomMovementVelocity = 10f;

    [SerializeField]
    private float fragmentRatio = 0.5f;

    [SerializeField]
    private int minAsteroidsSpawn = 3;
    [SerializeField]
    private int maxAsteroidsSpawn = 6;

    [SerializeField]
    private int minFragments = 2;
    [SerializeField]
    private int maxFragments = 3;

    void Start()
    {
        SpawnAsteroids();
	}

    public float GetFragmentRatio()
    {
        return fragmentRatio;
    }

    private void SpawnAsteroid(int asteroidTypeIdx, Vector3 spawnPosition, int size)
    {
        GameObject asteroid = Instantiate(asteroidPrefabs[asteroidTypeIdx], spawnPosition, Quaternion.Euler(Vector3.zero)) as GameObject;

        asteroid.GetComponent<AsteroidManager>().Spawn(size);
        RandomMover randomMover = asteroid.GetComponent<RandomMover>();
        randomMover.SetTumble(randomRotationTumble);
        randomMover.SetVelocity(randomMovementVelocity);
    }

    private void SpawnAsteroids()
    {
        int asteroidsCount = Random.Range(minAsteroidsSpawn, maxAsteroidsSpawn + 1);
        for (int i = 0; i < asteroidsCount; i++)
        {
            int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
            int asteroidSize = Random.Range(1, 4);

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

            SpawnAsteroid(asteroidTypeIdx, randomPositionOnScreenBorder, asteroidSize);
        }
    }

    public void SpawnFragments(Vector3 spawnPosition, int originalAsteroidSize)
    {
        int fragmentSize;
        if (originalAsteroidSize == 1)
        {
            // the asteroid was already of the smallest size
            // and should generate no fragments
            return;
        }
        fragmentSize = originalAsteroidSize - 1;

        int fragmentCount = Random.Range(minFragments, maxFragments + 1);
        for (int i = 0; i < fragmentCount; i++)
        {
            int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
            SpawnAsteroid(asteroidTypeIdx, spawnPosition, fragmentSize);
        }
    }
}
