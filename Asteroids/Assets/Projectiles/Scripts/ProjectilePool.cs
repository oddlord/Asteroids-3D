using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Initial pool amount")]
    [SerializeField]
    private int initialPoolAmount = 15;

    [Header("Projectile")]
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private Transform projectileMagazine; // this where to have the pooled object under one parent
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
        GameObject obj = Instantiate(projectilePrefab) as GameObject;
        obj.transform.SetParent(projectileMagazine);
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
