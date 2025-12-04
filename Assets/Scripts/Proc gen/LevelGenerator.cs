using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject checkpointChunkPrefab;
    [SerializeField] Transform chunkParent;
    [SerializeField] ScoreManager scoreManager;

    [Header("Level Settings")]
    [Tooltip("The amount of chunks we start with")]
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] int checkpointChunkInterval = 8;
    [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")]
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;

    [Header("Object Pool Settings")]
    [SerializeField] int poolSize = 20;

    List<GameObject> chunks = new List<GameObject>();
    int chunksSpawned = 0;

    Dictionary<GameObject, Queue<GameObject>> chunkPools = new Dictionary<GameObject, Queue<GameObject>>();

    void Start()
    {
        InitializePool();
        SpawnStartingChunks();
    }

    void InitializePool()
    {
        // CheckPoint Spawn
        chunkPools[checkpointChunkPrefab] = new Queue<GameObject>();
        CreatePooledChunks(checkpointChunkPrefab, poolSize / 4);

        // chunk spawn
        foreach (GameObject chunkPrefab in chunkPrefabs)
        {
            chunkPools[chunkPrefab] = new Queue<GameObject>();
            CreatePooledChunks(chunkPrefab, poolSize / chunkPrefabs.Length);
        }
    }

    void CreatePooledChunks(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject chunk = Instantiate(prefab, chunkParent);
            chunk.SetActive(false);
            chunkPools[prefab].Enqueue(chunk);
        }
    }

    GameObject GetChunkFromPool(GameObject prefab)
    {
        if (chunkPools[prefab].Count > 0)
        {
            GameObject chunk = chunkPools[prefab].Dequeue();
            chunk.SetActive(true);
            return chunk;
        }
        else
        {
            GameObject chunk = Instantiate(prefab, chunkParent);
            return chunk;
        }
    }

    void ReturnChunkToPool(GameObject chunk, GameObject prefab)
    {
        chunk.SetActive(false);
        chunkPools[prefab].Enqueue(chunk);
    }

    void Update() 
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if (newMoveSpeed != moveSpeed) 
        {
            moveSpeed = newMoveSpeed;

            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);
            
            cameraController.ChangeCameraFOV(speedAmount);
        }
    }

    void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunk();
        }
    }

    private void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPositionZ();
        Vector3 chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        GameObject chunkPrefab = ChooseChunkToSpawn();
        GameObject newChunkGO = GetChunkFromPool(chunkPrefab);
        newChunkGO.transform.position = chunkSpawnPos;
        newChunkGO.transform.rotation = Quaternion.identity;

        ChunkPoolInfo poolInfo = newChunkGO.GetComponent<ChunkPoolInfo>();
        if (poolInfo == null)
        {
            poolInfo = newChunkGO.AddComponent<ChunkPoolInfo>();
        }
        poolInfo.originalPrefab = chunkPrefab;
        
        chunks.Add(newChunkGO);
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager);

        chunksSpawned++;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;

        if (chunksSpawned % checkpointChunkInterval == 0 && chunksSpawned != 0)
        {
            chunkToSpawn = checkpointChunkPrefab;
        }
        else
        {
            chunkToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
        }

        return chunkToSpawn;
    }

    float CalculateSpawnPositionZ()
    {
        float spawnPositionZ;

        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ;
    }

   void MoveChunks() 
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];
            chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.RemoveAt(i);
                ChunkPoolInfo poolInfo = chunk.GetComponent<ChunkPoolInfo>();
                if (poolInfo != null && poolInfo.originalPrefab != null)
                {
                    ReturnChunkToPool(chunk, poolInfo.originalPrefab);
                }
                else
                {
                    Destroy(chunk);
                }
                
                SpawnChunk();
            }
        }
    }
}


public class ChunkPoolInfo : MonoBehaviour
{
    public GameObject originalPrefab;
}
