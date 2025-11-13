using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] obstaclePrefabs;
    [SerializeField] float obstacleSpawnTime = 1f;
    [SerializeField] float minObstacleSpawnTime = 0.2f;
    [SerializeField] Transform ObstacleParent;
    [SerializeField] float spawnWidth = 4f;


    void Start()
    {
        StartCoroutine(SpawnObstacles());
    }
    public void DecreaseobstacleSpawnTime(float amount)
    {
        obstacleSpawnTime -= amount;
        if (obstacleSpawnTime <= minObstacleSpawnTime)
        {
            obstacleSpawnTime = minObstacleSpawnTime;
        }
    }
    IEnumerator SpawnObstacles()
    {
        while (true)
        {
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Vector3 spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
            yield return new WaitForSeconds(obstacleSpawnTime);
            Instantiate(obstaclePrefab, spawnPosition, Random.rotation, ObstacleParent);
            
        }
    }
}
