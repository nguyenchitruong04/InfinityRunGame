using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{   
    private static ScoreManager _Instance;
    public static ScoreManager Instance => _Instance;

    [SerializeField] GameManger gameManger;
    [SerializeField] TMP_Text scoreText;

    int score = 0;
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
    public void AddScore(int amount)
    {
        if (gameManger.GameOver) return;
        score += amount;
        scoreText.text = score.ToString();
    }
}
