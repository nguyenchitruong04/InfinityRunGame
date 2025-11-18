using TMPro;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    [SerializeField] TMP_Text timeText;
    [SerializeField] GameObject gameOverText;
    [SerializeField] float startTime = 5f;
    [SerializeField] GameObject GOmenu;

    float timeLeft;
    bool gameOver = false;

    public bool GameOver => gameOver;

    void Start() 
    {
        timeLeft = startTime;
        GameEvents.OnCheckpointReached += HandleCheckpointReached;
    }

    void OnDestroy()
    {
        GameEvents.OnCheckpointReached -= HandleCheckpointReached;
    }

    void HandleCheckpointReached(float timeExtension, float spawnDecreaseAmount)
    {
        IncreaseTime(timeExtension);
        Debug.Log($"[GameManager] Received checkpoint event: +{timeExtension}s");
    }

    void Update()
    {
        DecreaseTime();
    }

    public void IncreaseTime(float amount) 
    {
        timeLeft += amount;
    }

    void DecreaseTime()
    {
        if (gameOver) return;

        timeLeft -= Time.deltaTime;
        timeText.text = timeLeft.ToString("F1");

        if (timeLeft <= 0f)
        {
            PlayerGameOver();
        }
    }

    void PlayerGameOver() 
    {
        gameOver = true;
        playerController.enabled = false;
        ScoreManager.Instance.SaveHighScore();
        GOmenu.SetActive(true);
        Time.timeScale = .1f;
        Cursor.visible = true;
    }
}
