using UnityEngine.Networking;
using UnityEngine;

public class SegmentSpawner : NetworkBehaviour {

	public GameObject segmentPrefab;

	private float spawnTimer;
	private float spawnInterval = 1f;
	private int segmentCount = 0;
	private int segmentCountMax = 100;

	void Start ()
	{
		spawnTimer = spawnInterval;
		if (isServer) {
			CmdSpawnSegments(segmentCountMax);
		}
	}

	void Update ()
	{
		if (isServer) {
			if (segmentCount < segmentCountMax) {

				if (spawnTimer <= 0f) {
					spawnTimer = spawnInterval;
					CmdSpawnSegments (1);
				} else {
					spawnTimer -= Time.deltaTime;
				}
			}
		}
	}

	[Command]
	public void CmdSpawnSegments (int count)
	{
		if (!isServer) {
			return;
		}
		float halfSafeSize = WallsController.safeSpawnDistance/2;

		for (int i = 0; i < count; i++) {
			Vector3 spawnPos = new Vector3 (Random.Range(-halfSafeSize, halfSafeSize), Random.Range(-halfSafeSize, halfSafeSize));
			Quaternion spawnRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
			var segment = Instantiate (segmentPrefab, spawnPos, spawnRot) as GameObject;
			segmentCount++;

			NetworkServer.Spawn (segment);
		}
	}

	[Command]
	public void CmdCollectSegment (GameObject segment)
	{
		segmentCount--;
		NetworkServer.Destroy (segment);
	}
}
