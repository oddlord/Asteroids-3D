using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    #region Private attributes
    private Asteroid asteroidData;
    #endregion

    #region Setters
    void SetSize(int s)
    {
        transform.localScale *= Mathf.Pow(AsteroidSpawner.Instance.GetFragmentRatio(), 3 - s);
        size = s;
    }
    #endregion

    #region Getters
    public int GetSize()
    {
        return size;
    }
    #endregion

    #region Spawn and Explode
    public void Spawn(int s)
    {
        SetSize(s);
    }

    public void Explode()
    {
        AsteroidSpawner.Instance.SpawnFragments(transform.position, size);
        Destroy(gameObject);
    }
    #endregion
}
