using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A spawn point tuple which pairs an object to spawn with a world location.
 */
[System.Serializable]
public class SpawnPoint {

    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField]
    private Transform spawnLocation;
    [SerializeField]
    private float spawnInterval;
    [SerializeField]
    private int countOfObjectsToSpawn;

    public GameObject GetObjectToSpawn() {
        return this.objectToSpawn;
    }

    public Transform GetSpawnLocation() {
        return this.spawnLocation;
    }

    public float GetSpawnInterval() {
        return this.spawnInterval;
    }

    public int GetCountOfObjectsToSpawn() {
        return this.countOfObjectsToSpawn;
    }
}
