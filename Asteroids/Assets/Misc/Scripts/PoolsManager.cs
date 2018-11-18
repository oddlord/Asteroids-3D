using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolsManager : MonoBehaviour
{
    #region Singleton pattern
    private static PoolsManager instance;

    public static PoolsManager Instance { get { return instance; } }

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
    [Header("Projectiles Pool")]
    [SerializeField]
    private int initialProjectiles = 15;
    [SerializeField]
    private Transform projectilesContainer;
    [SerializeField]
    private GameObject projectilePrefab;

    [Header("Life Icons Pool")]
    [SerializeField]
    private int initialLifeIcons = 3;
    [SerializeField]
    private Transform lifeIconsContainer;
    [SerializeField]
    private GameObject lifeIconPrefab;

    [Header("Asteroids Pool")]
    [SerializeField]
    private int initialAsteroids = 20;
    [SerializeField]
    private Transform asteroidsContainer;
    [SerializeField]
    private List<GameObject> asteroidPrefabs;

    [Header("Asteroid Audio Sources Pool")]
    [SerializeField]
    private int initialAsteroidAudioSources = 5;
    [SerializeField]
    private Transform asteroidAudioSourcesContainer;
    [SerializeField]
    private GameObject asteroidAudioSourcePrefab;
    #endregion

    #region Private attributes
    private Pool projectilesPool;
    private Pool lifeIconsPool;
    private Pool asteroidsPool;
    private Pool asteroidAudioSourcesPool;
    #endregion

    #region Start
    private void Start()
    {
        projectilesPool = new Pool(initialProjectiles, projectilesContainer, projectilePrefab);
        lifeIconsPool = new Pool(initialLifeIcons, lifeIconsContainer, lifeIconPrefab);
        asteroidsPool = new Pool(initialAsteroids, asteroidsContainer, asteroidPrefabs);
        asteroidAudioSourcesPool = new Pool(initialAsteroidAudioSources, asteroidAudioSourcesContainer, asteroidAudioSourcePrefab);
    }
    #endregion

    #region Getters
    public Pool GetProjectilesPool()
    {
        return projectilesPool;
    }

    public Pool GetLifeIconsPool()
    {
        return lifeIconsPool;
    }

    public Pool GetAsteroidsPool()
    {
        return asteroidsPool;
    }

    public Pool GetAsteroidAudioSourcesPool()
    {
        return asteroidAudioSourcesPool;
    }
    #endregion
}
