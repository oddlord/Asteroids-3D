using UnityEngine;
using UnityEngine.UI;

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
    [Header("Score Text")]
    [SerializeField]
    private Text scoreText;

    [Header("Points rewards")]
    [SerializeField]
    private int pointsForNewLife = 10000;
    #endregion

    #region Private attributes
    private int score;
    #endregion

    #region Init
    public void Init()
    {
        score = 0;
        UpdateScoreText();
    }
    #endregion

    #region Update Score
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }

    public void UpdateScore(int points)
    {
        int oldScore = score;
        score += points;
        
        if ((score / pointsForNewLife) > (oldScore / pointsForNewLife))
        {
            LivesManager.Instance.AddLife();
        }

        UpdateScoreText();
    }
    #endregion

    #region Getters
    public int GetScore()
    {
        return score;
    }
    #endregion
}
