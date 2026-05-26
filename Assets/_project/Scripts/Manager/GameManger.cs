using TMPro;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance { get; private set; }
    [SerializeField] private PlayerController playerController;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private float startTime = 5f;
    [SerializeField] private GameObject GOmenu;
    [SerializeField] private GameObject mainGameUI;

    float timeLeft;
    bool gameOver = false;

    public bool GameOver => gameOver;

    void Awake()
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
        mainGameUI.SetActive(false);
        Time.timeScale = .1f;
        Cursor.visible = true;
    }
}
