using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject applePrefab;
    [SerializeField] GameObject coinPrefab;
    [SerializeField] float appleSpawnChance = 0.5f;
    [SerializeField] float coinSpawnChance = 0.25f;
    [SerializeField] float coinSeparationLength = 1f;
    [SerializeField] float[] lanes = new float[] { -2.5f, 0f, 2.5f };
    [SerializeField] public float chunkLength = 10f;
    
    List<int> availableLanes = new List<int> { 0, 1, 2 };
    List<GameObject> spawnedObjects = new List<GameObject>(); // Track spawned objects
    LevelGenerator levelGen;
    ScoreManager scoreManager;
    bool isInitialized = false;
    bool shouldSpawnContent = true;
    
    void Start()
    {
        if (isInitialized)
        {
            return;
        }

        if (shouldSpawnContent)
        {
            SpawnChunkContent();
        }

        isInitialized = true;
    }
    
    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager, bool spawnContent = true)
    {
        this.levelGen = levelGenerator;
        this.scoreManager = scoreManager;
        shouldSpawnContent = spawnContent;
    }
    

    void OnEnable()
    {
        if (isInitialized)
        {
            ResetChunk();
        }
    }
    

    void ResetChunk()
    {
        ClearSpawnedObjects();
        availableLanes.Clear();
        availableLanes.AddRange(new int[] { 0, 1, 2 });

        if (shouldSpawnContent)
        {
            SpawnChunkContent();
        }
    }
    
    void ClearSpawnedObjects()
    {
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        spawnedObjects.Clear();
    }
    
    void SpawnChunkContent()
    {
        spawnFences();
        spawnApple();
        spawnCoins();
    }
    void spawnFences()
    {
        int fencesToSpawn = Random.Range(0, 3);
        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;
            int selectedLane = SelectRandomLane(availableLanes);
            Vector3 spawnPosition1 = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            GameObject fence = Instantiate(fencePrefab, spawnPosition1, Quaternion.identity, this.transform);
            spawnedObjects.Add(fence);
        }
    }

    int SelectRandomLane(List<int> availableLanes)
    {
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }


    void spawnApple()
    {
        if (Random.value > appleSpawnChance || availableLanes.Count <= 0) return;
        int selectedLane = SelectRandomLane(availableLanes);
        Vector3 spawnPosition1 = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        GameObject appleObj = Instantiate(applePrefab, spawnPosition1, Quaternion.identity, this.transform);
        Apple apple = appleObj.GetComponent<Apple>();
        apple.Init(levelGen);
        spawnedObjects.Add(appleObj); 
    }
  
    void spawnCoins()
    {
        if (Random.value > coinSpawnChance || availableLanes.Count <= 0) return;
        int selectedLane = SelectRandomLane(availableLanes);
        int maxCoins = 6;
        int cointoSpawn = Random.Range(1, maxCoins);
        float topOfChunkZ = transform.position.z + (coinSeparationLength * 2f);
        for (int i = 0; i < cointoSpawn; i++)
        {
            float spawnPositionZ = topOfChunkZ - (i * coinSeparationLength);
            Vector3 spawnPosition1 = new Vector3(lanes[selectedLane], transform.position.y + 0.5f, spawnPositionZ);
            GameObject coinObj = Instantiate(coinPrefab, spawnPosition1, Quaternion.identity, this.transform);
            spawnedObjects.Add(coinObj); 
        }
    }

}
