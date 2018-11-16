using System.Collections.Generic;
using UnityEngine;

public class AsteroidAudioSourcePool : MonoBehaviour
{
    #region SerializeField attributes
    [Header("Initial pool amount")]
    [SerializeField]
    private int initialPoolAmount = 5;

    [Header("Asteroids")]
    [SerializeField]
    private GameObject audioSourcePrefab;
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
        GameObject obj = Instantiate(audioSourcePrefab) as GameObject;
        obj.transform.SetParent(transform, false);
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
