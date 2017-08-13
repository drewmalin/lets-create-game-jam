using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The scene manager is responsible for manging the enemy spawn points.
 */
public class SceneManager : MonoBehaviour {

    [SerializeField]
    private List<SpawnPoint> enemySpawnPoints;
    [SerializeField]
    private DamagePopUp damagePopUp;

	void Start () {
        DamagePopUpController.Initialize (this.damagePopUp);
        SpawnEnemies ();
	}

    private void SpawnEnemies() {
        foreach (SpawnPoint spawnPoint in this.enemySpawnPoints) {
            StartCoroutine (SpawnEnemies (spawnPoint));
        }
    }

    private IEnumerator SpawnEnemies(SpawnPoint spawnPoint) {
        for (int i = spawnPoint.GetCountOfObjectsToSpawn (); i > 0; i--) {
            Instantiate (spawnPoint.GetObjectToSpawn (), spawnPoint.GetSpawnLocation ().position, spawnPoint.GetSpawnLocation ().rotation);
            yield return new WaitForSeconds (spawnPoint.GetSpawnInterval ());
        }
    }
}
