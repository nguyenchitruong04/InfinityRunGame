using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float checkpointTimeExtension = 5f;
    [SerializeField] float obstacleSpawnDecreaseAmount = 0.2f;

    const string playerString = "Player";
    bool hasBeenTriggered = false; // Tránh trigger nhiều lần

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(playerString) && !hasBeenTriggered) 
        {
            hasBeenTriggered = true;
            GameEvents.CheckpointReached(checkpointTimeExtension, obstacleSpawnDecreaseAmount);
            Debug.Log($"Checkpoint triggered! Time+{checkpointTimeExtension}s");
        }    
    }
    void OnDisable()
    {
        hasBeenTriggered = false;
    }
}

