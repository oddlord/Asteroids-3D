using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    #region Private attributes
    private AsteroidData asteroidData;
    #endregion

    #region Setters
    public void SetAsteroidData(AsteroidData ad)
    {
        asteroidData = ad;
        transform.localScale *= asteroidData.GetScale();
    }
    #endregion

    #region Getters

    #endregion

    #region Explode
    public void Explode()
    {
        AsteroidSpawner.Instance.SpawnFragments(transform.position, asteroidData);
        ScoreManager.Instance.UpdateScore(asteroidData.GetPoints());
        Destroy(gameObject);
    }
    #endregion
}
