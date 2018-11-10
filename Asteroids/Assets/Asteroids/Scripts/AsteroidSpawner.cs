using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> asteroidPrefabs;

    [SerializeField]
    private int asteroidSizes = 3;
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

    private Camera cam;
    
	void Start()
    {
        cam = Camera.main;
        SpawnAsteroids();
	}

    public int GetAsteroidSizes()
    {
        return asteroidSizes;
    }

    public float GetFragmentRatio()
    {
        return fragmentRatio;
    }

    void SpawnAsteroids()
    {
        int asteroidsCount = Random.Range(minAsteroidsSpawn, maxAsteroidsSpawn + 1);
        for (int i = 0; i < asteroidsCount; i++)
        {
            int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
            int asteroidSize = Random.Range(1, asteroidSizes + 1);

            float randomViewportX = Random.Range(0f, 1f);
            float randomViewportY = Random.Range(0f, 1f);
            Vector2 randomPositionOnScreen = Camera.main.ScreenToViewportPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0f));

            Debug.Log(randomPositionOnScreen);

            GameObject asteroid = Instantiate(asteroidPrefabs[asteroidTypeIdx], randomPositionOnScreen, Quaternion.Euler(Vector3.zero)) as GameObject;
            asteroid.GetComponent<AsteroidController>().Spawn(this, asteroidSize);
        }
    }

    public void SpawnFragments(Vector3 spawnPosition, int size)
    {
        int fragmentSize;
        if (size == 1)
        {
            // the asteroid was already of the smallest size
            return;
        }
        fragmentSize = size - 1;

        int fragmentCount = Random.Range(minFragments, maxFragments + 1);
        for (int i = 0; i < fragmentCount; i++)
        {
            int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
            GameObject asteroid = Instantiate(asteroidPrefabs[asteroidTypeIdx], spawnPosition, Quaternion.Euler(Vector3.zero)) as GameObject;
            asteroid.GetComponent<AsteroidController>().Spawn(this, fragmentSize);
        }
    }
}
