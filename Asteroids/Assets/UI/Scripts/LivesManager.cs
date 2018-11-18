using UnityEngine;

[RequireComponent(typeof(AudioSource))]
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
    #endregion

    #region Private attributes
    private AudioSource audioSource;
    private int lives;
    #endregion

    #region Init
    public void Init()
    {
        audioSource = GetComponent<AudioSource>();

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
        int activeLifeIcons = 0;
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (child.activeInHierarchy)
            {
                if (activeLifeIcons == lives)
                {
                    PoolsManager.Instance.GetLifeIconsPool().SetAvailable(child);
                }
                else
                {
                    activeLifeIcons++;
                }
            }
        }

        for (int i = activeLifeIcons; i < lives; i++)
        {
            GameObject lifeIcon = PoolsManager.Instance.GetLifeIconsPool().GetAvailable();
            lifeIcon.SetActive(true);
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
        SoundManager.Instance.PlaySFX(audioSource);
        UpdateLivesCount();
    }
    #endregion
}
