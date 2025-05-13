using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public GameObject prefabToSpawn;
    public int amount = 10;
    public float prefabSize = 1f;
    public float totalSpawnTime = 10f;
    public float fixedYPosition = 1f;

    public float minX = -10f;
    public float maxX = -8f;
    public float minZ = -5f;
    public float maxZ = 5f;

    void Start()
    {
        StartCoroutine(SpawnPrefabsGradually());
    }

    IEnumerator SpawnPrefabsGradually()
    {
        if (prefabToSpawn != null)
        {
            List<Vector3> spawnPositions = new List<Vector3>();

            float spawnInterval = totalSpawnTime / amount;

            for (int i = 0; i < amount; i++)
            {
                Vector3 spawnPosition;
                bool positionFound = false;

                for (int attempt = 0; attempt < 100; attempt++)
                {
                    spawnPosition = new Vector3(
                        Random.Range(minX, maxX),
                        fixedYPosition,
                        Random.Range(minZ, maxZ)
                    );

                    if (!IsOverlapping(spawnPosition, spawnPositions, prefabSize))
                    {
                        spawnPositions.Add(spawnPosition);
                        Instantiate(prefabToSpawn, spawnPosition, Quaternion.identity);
                        positionFound = true;
                        break;
                    }
                }

                if (!positionFound)
                {
                    Debug.LogWarning("Could not find a valid position for prefab " + i);
                }

                yield return new WaitForSeconds(spawnInterval);
            }
        }
        else
        {
            Debug.LogWarning("Prefab to spawn is not assigned.");
        }
    }

    bool IsOverlapping(Vector3 newPos, List<Vector3> existingPositions, float size)
    {
        foreach (var pos in existingPositions)
        {
            if (Vector3.Distance(newPos, pos) < size)
            {
                return true;
            }
        }
        return false;
    }
}