using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    #region Private attributes
    private int initialAmount;

    private bool singlePrefab;
    private GameObject prefab;
    private List<GameObject> prefabs;

    private Transform container;

    private Queue<GameObject> available;
    #endregion

    #region Constructors
    public Pool(int initialAmount, Transform container, GameObject prefab)
    {
        singlePrefab = true;
        this.prefab = prefab;

        InitPool(initialAmount, container);
    }

    public Pool(int initialAmount, Transform container, List<GameObject> prefabs)
    {
        singlePrefab = false;
        this.prefabs = prefabs;

        InitPool(initialAmount, container);
    }
    #endregion

    #region Utility function
    private void InitPool(int initialAmount, Transform container)
    {
        this.initialAmount = initialAmount;
        this.container = container;

        available = new Queue<GameObject>();
        for (int i = 0; i < this.initialAmount; i++)
        {
            AddNew();
        }
    }
    #endregion

    #region Add new element
    private void AddNew()
    {
        GameObject obj;
        if (singlePrefab)
        {
            obj = GameObject.Instantiate(prefab) as GameObject;
        }
        else
        {
            int prefabIdx = Random.Range(0, prefabs.Count);
            obj = GameObject.Instantiate(prefabs[prefabIdx]) as GameObject;
        }
        obj.transform.SetParent(container, false);
        obj.SetActive(false);
        available.Enqueue(obj);
    }
    #endregion

    #region Getters
    // does NOT set the GameObject as active
    // since additional operations might be needed
    // to be performed on the GameObject
    // before activating it
    public GameObject GetAvailable()
    {
        if (available.Count == 0)
        {
            // the current pool is not enough
            // there is need for more elements
            AddNew();
        }
        GameObject obj = available.Dequeue();
        return obj;
    }
    #endregion

    #region Setters
    public void SetAvailable(GameObject obj)
    {
        obj.SetActive(false);
        available.Enqueue(obj);
    }
    #endregion
}
