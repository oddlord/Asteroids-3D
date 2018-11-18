using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    #region Private attributes
    private AsteroidData asteroidData;
    private Vector3 originalLocalScale;
    private AudioSource audioSource;
    #endregion

    #region Awake and Start
    private void Awake()
    {
        originalLocalScale = transform.localScale;
    }
    #endregion

    #region Init
    public void Init(AsteroidData ad)
    {
        asteroidData = ad;
        transform.localScale = originalLocalScale * asteroidData.GetScale();
    }
    #endregion

    #region Explode
    public void Explode()
    {
        GameObject asteroidAudioSource = PoolsManager.Instance.GetAsteroidAudioSourcesPool().GetAvailable();
        asteroidAudioSource.transform.position = transform.position;
        asteroidAudioSource.SetActive(true);

        AsteroidSpawner.Instance.SpawnFragments(transform.position, asteroidData);
        ScoreManager.Instance.UpdateScore(asteroidData.GetPoints());
        PoolsManager.Instance.GetAsteroidsPool().SetAvailable(gameObject);
    }
    #endregion
}
