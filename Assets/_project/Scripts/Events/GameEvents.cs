using UnityEngine;
using System;

public static class GameEvents
{
    public static event Action<float, float> OnCheckpointReached;
    public static event Action OnGameOver;
    public static event Action<int> OnScoreChanged;
    public static event Action<float> OnSpeedChanged;
    public static void CheckpointReached(float timeExtension, float spawnDecreaseAmount)
    {
        OnCheckpointReached?.Invoke(timeExtension, spawnDecreaseAmount);
        Debug.Log($"[GameEvents] Checkpoint reached! Time+{timeExtension}, SpawnDecrease-{spawnDecreaseAmount}");
    }
    public static void GameOver()
    {
        OnGameOver?.Invoke();
        Debug.Log("[GameEvents] Game Over!");
    }
    public static void ScoreChanged(int newScore)
    {
        OnScoreChanged?.Invoke(newScore);
    }
    public static void SpeedChanged(float speedAmount)
    {
        OnSpeedChanged?.Invoke(speedAmount);
    }
    public static void ClearAllEvents()
    {
        OnCheckpointReached = null;
        OnGameOver = null;
        OnScoreChanged = null;
        OnSpeedChanged = null;
    }
}
