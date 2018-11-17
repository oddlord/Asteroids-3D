using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Pool Settings")]
    [SerializeField]
    private int initialPoolAmount = 20;

    [Header("Asteroids")]
    [SerializeField]
    private List<GameObject> asteroidPrefabs;
    #endregion

    #region Private attributes
    private List<GameObject> pool;
    #endregion

    #region Init
    public void InitPool()
    {
        if (pool == null)
        {
            pool = new List<GameObject>();
            for (int i = 0; i < initialPoolAmount; i++)
            {
                AddNew();
            }
        }
        else
        {
            Debug.LogError("Pool already initialised!");
        }
    }
    #endregion

    #region Utility functions
    private GameObject AddNew()
    {
        int asteroidTypeIdx = Random.Range(0, asteroidPrefabs.Count);
        GameObject obj = Instantiate(asteroidPrefabs[asteroidTypeIdx]) as GameObject;
        obj.transform.SetParent(transform, false);
        obj.tag = GameManager.Instance.GetAsteroidTag();
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
    #endregion

    #region Get Available
    public GameObject GetAvailable()
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
            {
                return pool[i];
            }
        }

        GameObject obj = AddNew();
        return obj;
    }
    #endregion
}
