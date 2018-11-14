using UnityEngine;

public class LivesManager : MonoBehaviour
{
    #region Singleton pattern
    private static LivesManager instance;

    public static LivesManager Instance { get { return instance; } }

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

    #region SerializeField attributes
    [Header("Initial Lives")]
    [SerializeField]
    private int initialLives = 3;

    [Header("Life Icon")]
    [SerializeField]
    private GameObject lifeIconPrefab;
    #endregion

    #region Private attributes
    private int lives;
    #endregion

    #region Start
    private void Start()
    {
        lives = initialLives;
        UpdateLivesCount();
    }
    #endregion

    #region Gameover
    public bool IsGameover()
    {
        return (lives == 0);
    }
    #endregion

    #region Lives update
    private void UpdateLivesCount()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Destroy(child);
        }

        for (int i = 0; i < lives; i++)
        {
            GameObject lifeIcon = Instantiate(lifeIconPrefab, lifeIconPrefab.transform.position, lifeIconPrefab.transform.rotation) as GameObject;
            lifeIcon.transform.SetParent(transform, false);
        }
    }

    public void RemoveLife()
    {
        lives--;
        UpdateLivesCount();
    }

    public void AddLife()
    {
        lives++;
        UpdateLivesCount();
    }
    #endregion
}
