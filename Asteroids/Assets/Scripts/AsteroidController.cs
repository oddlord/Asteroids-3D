using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    #region Private attributes
    private AsteroidData asteroidData;
    private Vector3 originalLocalScale;
    #endregion

    #region Awake
    private void Awake()
    {
        originalLocalScale = transform.localScale;
    }
    #endregion

    #region Setters
    public void SetAsteroidDataAndScale(AsteroidData ad)
    {
        asteroidData = ad;
        transform.localScale = originalLocalScale * asteroidData.GetScale();
    }
    #endregion

    #region Getters

    #endregion

    #region Explode
    public void Explode()
    {
        AsteroidSpawner.Instance.SpawnFragments(transform.position, asteroidData);
        ScoreManager.Instance.UpdateScore(asteroidData.GetPoints());
        gameObject.SetActive(false);
    }
    #endregion
}
