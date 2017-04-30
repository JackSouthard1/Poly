using UnityEngine.Networking;
using UnityEngine;

public class PartSpawner : NetworkBehaviour {

	public GameObject partPrefab;
//	private GameObject segments;

	private float spawnTimer;
	private float spawnInterval = 2f;
	private int partCount = 0;
	private int partCountMax = 5;
	// Use this for initialization
	void Start () {
		spawnTimer = spawnInterval;
	}
	
	void Update ()
	{
		if (spawnTimer <= 0f) {
			spawnTimer = spawnInterval;
			SpawnSegment ();
		} else {
			spawnTimer -= Time.deltaTime;
		}
	}
	void SpawnSegment ()
	{
		if (partCount < partCountMax) {
			var part = Instantiate (partPrefab, Vector3.zero, Quaternion.identity) as GameObject;
//				segment.transform.parent = this.gameObject.transform;
			partCount++;

			NetworkServer.Spawn (part);
		}
	}

	[Command]
	public void CmdPartDestroyed (GameObject part)
	{
		partCount--;
		NetworkServer.Destroy (part);
	}
}
