using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlueSpawn : MonoBehaviour
{
    public GameObject FireBallPrefab;
    public float minSpawnDelay = 1f;
    public float maxSpawnDelay = 3f;
    public float spawnXLimit = 6f;

    void Start()
    {
        Spawn();
    }

    void Spawn()
    {
        float random = Random.Range(-spawnXLimit, spawnXLimit);
        Vector3 spawnPos = transform.position + new
        Vector3(random, 0f, 0f);
        Instantiate(FireBallPrefab, spawnPos, Quaternion.identity);
        Invoke("Spawn", Random.Range(minSpawnDelay, maxSpawnDelay));
    }
}
