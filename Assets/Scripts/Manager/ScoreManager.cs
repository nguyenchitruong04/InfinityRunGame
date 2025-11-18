using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    private static ScoreManager _Instance;
    public static ScoreManager Instance => _Instance;

    [SerializeField] GameManger gameManger;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text highScoreText;

    int score = 0;
    int highScore = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            _Instance = this;
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
        if (gameManger.GameOver) return;
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
