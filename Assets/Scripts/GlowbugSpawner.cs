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
    private LinkedList<Glowbug> spawnedGlowbugs = new LinkedList<Glowbug>();

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
        spawnedGlowbugs.AddLast(glowbug);
        glowbug.transform.name = "Glowbug (" + spawnedGlowbugs.Count + ")";
        glowbug.Randomize();
    }
}
