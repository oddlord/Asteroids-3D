using UnityEngine;

[CreateAssetMenu(fileName = "AsteroidData", menuName = "Asteroids/AsteroidData", order = 1)]
public class AsteroidData : ScriptableObject
{
    #region SerializeField attributes
    [Header("Asteroid Physics")]
    [SerializeField]
    private float mass = 100f;
    [SerializeField]
    private float scale = 1f;

    [Header("Points")]
    [SerializeField]
    private int points = 20;

    [Header("Fragments")]
    [SerializeField]
    private int numberOfFragments;
    [SerializeField]
    private AsteroidData fragmentToSpawn;
    #endregion

    #region Getters
    public float GetScale()
    {
        return scale;
    }

    public float GetMass()
    {
        return mass;
    }

    public int GetPoints()
    {
        return points;
    }

    public int GetNumberOfFragments()
    {
        return numberOfFragments;
    }

    public AsteroidData GetFragmentToSpawn()
    {
        return fragmentToSpawn;
    }
    #endregion
}
