using System.Collections.Generic;
using UnityEngine;

public class LifeIconPool : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Initial pool amount")]
    [SerializeField]
    private int initialPoolAmount = 4;

    [Header("Life Icon")]
    [SerializeField]
    private GameObject lifeIconPrefab;
    #endregion

    #region Private attributes
    private List<GameObject> pool;
    #endregion

    #region Init
    private void InitPool()
    {
        pool = new List<GameObject>();
        for (int i = 0; i < initialPoolAmount; i++)
        {
            AddNew();
        }
    }
    #endregion

    #region Utility functions
    private GameObject AddNew()
    {
        GameObject obj = Instantiate(lifeIconPrefab) as GameObject;
        obj.transform.SetParent(transform, false);
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
    #endregion

    #region Get Available
    public GameObject GetAvailable()
    {
        if (pool == null)
        {
            InitPool();
        }

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
