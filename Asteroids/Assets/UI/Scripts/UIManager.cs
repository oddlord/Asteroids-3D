using UnityEngine;

public class UIManager : MonoBehaviour
{
    #region Singleton pattern
    private static UIManager instance;

    public static UIManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    #endregion

    public void AddElement(GameObject element)
    {
        element.transform.SetParent(transform, false);
    }
}
