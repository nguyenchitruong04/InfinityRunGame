using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] float checkpointTimeExtension = 5f;
    [SerializeField] float obstacleSpawnDecreaseAmount = 0.2f;

    GameManger gameManager;
    ObstacleSpawner obstacleSpawner;
   
    const string playerString = "Player";

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManger>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag(playerString)) 
        {
            if (gameManager != null)
            {
                gameManager.IncreaseTime(checkpointTimeExtension);
                obstacleSpawner.DecreaseobstacleSpawnTime(obstacleSpawnDecreaseAmount);

            }
           
           
        }    
    }
}

