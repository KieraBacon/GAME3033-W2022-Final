using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowbugSpawner : MonoBehaviour
{
    [SerializeField]
    private float spawnRadius;
    [SerializeField]
    private Glowbug glowbugPrefab;
    [SerializeField]
    private int glowbugsToSpawn;

    private void Start()
    {
        for (int i = 0; i < glowbugsToSpawn; i++)
        {
            SpawnGlowbug();
        }
    }

    private void SpawnGlowbug()
    {
        Vector2 randomRadialPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomRadialPosition.x, 0, randomRadialPosition.y);
        Quaternion spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        Glowbug glowbug = Instantiate(glowbugPrefab, spawnPosition, spawnRotation, transform);
        glowbug.Randomize();
    }
}
