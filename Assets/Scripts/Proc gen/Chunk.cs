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
    List<int> availableLanes = new List<int> { 0, 1, 2 };
    LevelGenerator levelGen;
    ScoreManager scoreManager;
    void Start()
    {
        spawnFences();
        spawnApple();
        spawnCoins();
    }
    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager)
    {
        this.levelGen = levelGenerator;
        this.scoreManager = scoreManager;
    }
    void spawnFences()
    {
        int fencesToSpawn = Random.Range(0, 3);
        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;
            int selectedLane = SelectRandomLane(availableLanes);
            Vector3 spawnPosition1 = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition1, Quaternion.identity, this.transform);

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
            Vector3 spawnPosition1 = new Vector3(lanes[selectedLane], transform.position.y, spawnPositionZ);
            GameObject coinObj = Instantiate(coinPrefab, spawnPosition1, Quaternion.identity, this.transform);
            
        }
    }
    

}
