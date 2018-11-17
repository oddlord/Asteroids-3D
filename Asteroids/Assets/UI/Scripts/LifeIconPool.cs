using System.Collections.Generic;
using UnityEngine;

public class LifeIconPool : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Pool Settings")]
    [SerializeField]
    private int initialPoolAmount = 4;

    [Header("Life Icon")]
    [SerializeField]
    private GameObject lifeIconPrefab;
    [SerializeField]
    private GameObject container;
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
        GameObject obj = Instantiate(lifeIconPrefab) as GameObject;
        obj.transform.SetParent(container.transform, false);
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
