using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreManager : MonoBehaviour
{
    #region Singleton pattern
    private static ScoreManager instance;

    public static ScoreManager Instance { get { return instance; } }

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
    [Header("Points rewards")]
    [SerializeField]
    private int pointsForNewLife = 10000;
    #endregion

    #region Private attributes
    private int score;
    private Text scoreText;
    #endregion

    #region Start
    private void Start()
    {
        score = 0;
        scoreText = GetComponent<Text>();
    }
    #endregion

    #region Update Score
    public void UpdateScore(int points)
    {
        int oldScore = score;
        score += points;
        
        if ((score / pointsForNewLife) > (oldScore / pointsForNewLife))
        {
            LivesManager.Instance.AddLife();
        }

        scoreText.text = score.ToString();
    }
    #endregion
}
