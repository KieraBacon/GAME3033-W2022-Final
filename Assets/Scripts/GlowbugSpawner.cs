using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowbugSpawner : MonoBehaviour
{
    public static HashSet<GlowbugSpawner> allGlowbugSpawners = new HashSet<GlowbugSpawner>();

    [SerializeField]
    private float spawnRadius;
    [SerializeField]
    private Glowbug glowbugPrefab;
    [SerializeField]
    private int glowbugsToSpawn;
    private List<Glowbug> active = new List<Glowbug>();
    private Queue<Glowbug> pool = new Queue<Glowbug>();

    private void OnEnable()
    {
        allGlowbugSpawners.Add(this);
    }

    private void OnDisable()
    {
        allGlowbugSpawners.Remove(this);
    }

    private void Add()
    {
        Glowbug glowbug = Instantiate(glowbugPrefab, transform);
        pool.Enqueue(glowbug);
        glowbug.gameObject.SetActive(false);
    }

    private Glowbug Fetch()
    {
        if (pool.Count < 1)
            Add();
        return pool.Dequeue();
    }

    private void Return(Glowbug glowbug)
    {
        glowbug.gameObject.SetActive(false);
        glowbug.transform.name = "Glowbug (Inactive)";
        pool.Enqueue(glowbug);
    }

    public void Spawn()
    {
        Vector2 randomRadialPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomRadialPosition.x, 0, randomRadialPosition.y);
        Quaternion spawnRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));

        Glowbug glowbug = Fetch();
        glowbug.transform.position = spawnPosition;
        glowbug.transform.rotation = spawnRotation;

        active.Add(glowbug);
        glowbug.transform.name = "Glowbug (" + active.Count + ")";
        glowbug.Randomize();
        glowbug.gameObject.SetActive(true);
    }


    public void SpawnAll()
    {
        foreach (Glowbug glowbug in active)
        {
            Return(glowbug);
        }
        active.Clear();

        for (int i = 0; i < glowbugsToSpawn; i++)
        {
            Spawn();
        }
    }

    public void Despawn(Glowbug glowbug)
    {
        active.Remove(glowbug);
        Return(glowbug);
    }

    public void DespawnRandom()
    {
        Despawn(active.ElementAt(Random.Range(0, active.Count - 1)));
    }
}
