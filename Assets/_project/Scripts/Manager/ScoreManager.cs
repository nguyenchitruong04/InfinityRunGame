using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    public static ScoreManager Instance { get; private set; }

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    int score = 0;
    int highScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        highScore = PlayerPrefs.GetInt("SavedHighScore", 0);
        highScoreText.text = highScore.ToString();
        scoreText.text = "0";
    }
    public void AddScore(int amount)
    {
        if (GameManger.Instance.GameOver) return;
        score += amount;
        scoreText.text = score.ToString();

        if (score > highScore)
        {
            highScore = score;
            highScoreText.text = highScore.ToString();
        }
    }

    public void SaveHighScore()
    {
        if (score > PlayerPrefs.GetInt("SavedHighScore", 0))
        {
            PlayerPrefs.SetInt("SavedHighScore", score);
            PlayerPrefs.Save();
        }
    }

    public int GetCurrentScore()
    {
        return score;
    }

    public void ResetScore()
    {
        score = 0;
        scoreText.text = "0";
    }
}
