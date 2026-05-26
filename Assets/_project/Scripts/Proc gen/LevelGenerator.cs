using System.Collections;
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
    [SerializeField, Min(1)] int startingChunksAmount = 12;
    [Tooltip("Starting chunks kept clear of generated objects so the player has a safe entry.")]
    [SerializeField, Min(0)] int clearStartingChunksAmount = 2;
    [Tooltip("Number of starting chunks created in one frame to avoid an initial frame spike.")]
    [SerializeField, Min(1)] int startingChunksPerFrame = 1;
    [SerializeField] int checkpointChunkInterval = 8;
    [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")]
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;

    [Header("Object Pool Settings")]
    [Tooltip("Maximum inactive chunks retained per prefab after they leave the track.")]
    [SerializeField, Min(0)] int poolSize = 20;

    List<GameObject> chunks = new List<GameObject>();
    int chunksSpawned = 0;

    Dictionary<GameObject, Queue<GameObject>> chunkPools = new Dictionary<GameObject, Queue<GameObject>>();

    void Start()
    {
        InitializePool();
        StartCoroutine(SpawnStartingChunks());
    }

    void InitializePool()
    {
        chunkPools[checkpointChunkPrefab] = new Queue<GameObject>();

        foreach (GameObject chunkPrefab in chunkPrefabs)
        {
            chunkPools[chunkPrefab] = new Queue<GameObject>();
        }
    }

    GameObject GetChunkFromPool(GameObject prefab)
    {
        if (chunkPools[prefab].Count > 0)
        {
            GameObject chunk = chunkPools[prefab].Dequeue();
            return chunk;
        }
        else
        {
            GameObject chunk = Instantiate(prefab, chunkParent);
            chunk.SetActive(false);
            return chunk;
        }
    }

    void ReturnChunkToPool(GameObject chunk, GameObject prefab)
    {
        chunk.SetActive(false);

        if (chunkPools[prefab].Count < poolSize)
        {
            chunkPools[prefab].Enqueue(chunk);
        }
        else
        {
            Destroy(chunk);
        }
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

    IEnumerator SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunk(i >= clearStartingChunksAmount);

            if ((i + 1) % Mathf.Max(1, startingChunksPerFrame) == 0)
            {
                yield return null;
            }
        }
    }

    private void SpawnChunk(bool spawnContent = true)
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
        newChunk.Init(this, scoreManager, spawnContent);
        newChunkGO.SetActive(true);

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
        if (chunks.Count == 0)
        {
            return transform.position.z;
        }
        GameObject lastChunkGO = chunks[chunks.Count - 1];
        Chunk lastChunkScript = lastChunkGO.GetComponent<Chunk>();
        return lastChunkGO.transform.position.z + lastChunkScript.chunkLength;
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
