using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class TreeSpawnerFromTilemap : MonoBehaviour
{
    public Tilemap groundTilemap;
    public GameObject treePrefab;
    public int numberOfTrees = 10;
    public float minDistanceBetweenTrees = 1.5f;
    void Start()
    {
        SpawnTreesOnTopGround();
    }

    void SpawnTreesOnTopGround()
    {
        List<Vector3> validPositions = new List<Vector3>();
        BoundsInt bounds = groundTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);

                if (groundTilemap.GetTile(pos) != null)
                {
                    Vector3Int posAbove = new Vector3Int(x, y + 1, 0);
                    if (groundTilemap.GetTile(posAbove) == null)
                    {
                        Vector3 worldPos = groundTilemap.GetCellCenterWorld(pos);
                        validPositions.Add(worldPos);
                    }
                }
            }
        }

        List<Vector3> spawnedTreePositions = new List<Vector3>();

        for (int i = 0; i < numberOfTrees; i++)
        {
            if (validPositions.Count == 0) break;

            int randomIndex = Random.Range(0, validPositions.Count);
            Vector3 spawnPos = validPositions[randomIndex];

            // ✅ Kiểm tra khoảng cách với cây đã spawn
            bool tooClose = false;
            foreach (Vector3 otherPos in spawnedTreePositions)
            {
                if (Vector3.Distance(spawnPos, otherPos) < minDistanceBetweenTrees)
                {
                    tooClose = true;
                    break;
                }
            }

            if (tooClose)
            {
                validPositions.RemoveAt(randomIndex);
                i--;
                continue;
            }

            Instantiate(treePrefab, spawnPos + new Vector3(0, 2f, 0), Quaternion.identity);
            spawnedTreePositions.Add(spawnPos);
            validPositions.RemoveAt(randomIndex);
        }
    }
}
