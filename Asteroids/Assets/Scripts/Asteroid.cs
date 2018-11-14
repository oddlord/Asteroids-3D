using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Asteroid", menuName = "Asteroids/Asteroid", order = 1)]
public class Asteroid : ScriptableObject
{
    #region SerializeField attributes
    [SerializeField]
    private float scale = 1f;

    [SerializeField]
    private int points = 20;

    [SerializeField]
    Asteroid asteroidToSpawn;
    #endregion

    #region Getters
    public float GetScale()
    {
        return scale;
    }

    public float GetMass()
    {
        return scale * 100;
    }

    public int GetPoints()
    {
        return points;
    }

    public Asteroid GetAsteroidToSpawn()
    {
        return asteroidToSpawn;
    }
    #endregion
}
