using UnityEngine.Networking;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour {

	public GameObject enemyPrefab;
	public int numberOfEnemies;

	public override void OnStartServer() {
		for (int i = 0; i < numberOfEnemies; i++) {
			var spawnPosition = new Vector3(Random.Range(-8f, 8f), Random.Range(-8f, 8f), 0);

			var spawnRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 180));
			var enemy = (GameObject)Instantiate(enemyPrefab, spawnPosition, spawnRotation);
			NetworkServer.Spawn(enemy);
		}
	}
}
