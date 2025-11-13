using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] GameManger gameManger;
    [SerializeField] TMP_Text scoreText;
    int score = 0;
    public void AddScore(int amount)
    {
        if (gameManger.GameOver) return;
        score += amount;
        scoreText.text = score.ToString();
    }
}
