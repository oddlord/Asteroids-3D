using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    [SerializeField]
    AsteroidSpawner asteroidSpawner;

    private int size;

    void SetSize(int s)
    {
        transform.localScale *= Mathf.Pow(asteroidSpawner.GetFragmentRatio(), asteroidSpawner.GetAsteroidSizes() - s);
        size = s;
    }

    public void Spawn(AsteroidSpawner asteroidS, int s)
    {
        asteroidSpawner = asteroidS;
        SetSize(s);
    }

    void Explode()
    {
        asteroidSpawner.SpawnFragments(transform.position, size);
        Destroy(this.gameObject);
    }
	
	void Update()
    {
		
	}
}
