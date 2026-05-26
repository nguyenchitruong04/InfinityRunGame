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
    
    [Header("Object Pool Settings")]
    [SerializeField] int poolSizePerPrefab = 10;
    [SerializeField] float obstacleLifetime = 10f;

    Dictionary<GameObject, Queue<GameObject>> obstaclePools = new Dictionary<GameObject, Queue<GameObject>>();
    List<ActiveObstacle> activeObstacles = new List<ActiveObstacle>();

    class ActiveObstacle
    {
        public GameObject obstacle;
        public GameObject prefab;
        public float spawnTime;
    }

    void Start()
    {
        InitializePool();
        StartCoroutine(SpawnObstacles());
        StartCoroutine(CheckObstacleLifetime());
        GameEvents.OnCheckpointReached += HandleCheckpointReached;
    }

    void OnDestroy()
    {
        GameEvents.OnCheckpointReached -= HandleCheckpointReached;
    }

    void HandleCheckpointReached(float timeExtension, float spawnDecreaseAmount)
    {
        DecreaseobstacleSpawnTime(spawnDecreaseAmount);
        Debug.Log($"[ObstacleSpawner] Received checkpoint event: Spawn time decreased by {spawnDecreaseAmount}s");
    }

    void InitializePool()
    {
        foreach (GameObject prefab in obstaclePrefabs)
        {
            obstaclePools[prefab] = new Queue<GameObject>();
            CreatePooledObstacles(prefab, poolSizePerPrefab);
        }
    }

    void CreatePooledObstacles(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obstacle = Instantiate(prefab, ObstacleParent);
            obstacle.SetActive(false);
            obstaclePools[prefab].Enqueue(obstacle);
        }
    }

    GameObject GetObstacleFromPool(GameObject prefab)
    {
        if (obstaclePools[prefab].Count > 0)
        {
            GameObject obstacle = obstaclePools[prefab].Dequeue();
            obstacle.SetActive(true);
            return obstacle;
        }
        else
        {
            GameObject obstacle = Instantiate(prefab, ObstacleParent);
            return obstacle;
        }
    }

    void ReturnObstacleToPool(GameObject obstacle, GameObject prefab)
    {
        obstacle.SetActive(false);
        obstaclePools[prefab].Enqueue(obstacle);
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
            yield return new WaitForSeconds(obstacleSpawnTime);
            
            GameObject obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnWidth, spawnWidth), 
                transform.position.y, 
                transform.position.z
            );
            
            GameObject newObstacle = GetObstacleFromPool(obstaclePrefab);
            newObstacle.transform.position = spawnPosition;
            newObstacle.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            if (newObstacle.TryGetComponent(out Rigidbody obstacleRigidbody))
            {
                obstacleRigidbody.linearVelocity = Vector3.zero;
                obstacleRigidbody.angularVelocity = Vector3.zero;
            }
            
            ActiveObstacle activeObs = new ActiveObstacle
            {
                obstacle = newObstacle,
                prefab = obstaclePrefab,
                spawnTime = Time.time
            };
            activeObstacles.Add(activeObs);
        }
    }

    IEnumerator CheckObstacleLifetime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            
            for (int i = activeObstacles.Count - 1; i >= 0; i--)
            {
                ActiveObstacle activeObs = activeObstacles[i];
                
                if (Time.time - activeObs.spawnTime > obstacleLifetime || 
                    activeObs.obstacle.transform.position.z < Camera.main.transform.position.z - 20f)
                {
                    ReturnObstacleToPool(activeObs.obstacle, activeObs.prefab);
                    activeObstacles.RemoveAt(i);
                }
            }
        }
    }

    public void ReturnObstacle(GameObject obstacle)
    {
        for (int i = activeObstacles.Count - 1; i >= 0; i--)
        {
            if (activeObstacles[i].obstacle == obstacle)
            {
                ReturnObstacleToPool(obstacle, activeObstacles[i].prefab);
                activeObstacles.RemoveAt(i);
                break;
            }
        }
    }
}
