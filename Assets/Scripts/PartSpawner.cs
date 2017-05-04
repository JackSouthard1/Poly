using UnityEngine.Networking;
using UnityEngine;

public class PartSpawner : NetworkBehaviour {

	public GameObject floatingPartPrefab;

	public GameObject[] attachedPartsList;

	private float spawnTimer;
	private float spawnInterval = 2f;
	private int partCount = 0;
	private int partCountMax = 20;

	void Start ()
	{
		spawnTimer = spawnInterval;
		if (isServer) {
			CmdSpawnParts(partCountMax);
		}
	}
	
	void Update ()
	{
		if (isServer) {
			if (partCount < partCountMax) {
				if (spawnTimer <= 0f) {
					spawnTimer = spawnInterval;
					CmdSpawnParts (1);
				} else {
					spawnTimer -= Time.deltaTime;
				}
			}
		}
	}

	[Command]
	public void CmdSpawnParts (int count)
	{
		float halfSafeSize = WallsController.safeSpawnDistance/2;

		for (int i = 0; i < count; i++) {
			Vector3 spawnPos = new Vector3 (Random.Range(-halfSafeSize, halfSafeSize), Random.Range(-halfSafeSize, halfSafeSize));
			Quaternion spawnRot = Quaternion.Euler(0, 0, Random.Range(0, 360));
			var part = Instantiate (floatingPartPrefab, spawnPos, spawnRot) as GameObject;
			part.GetComponent<FloatingPartController> ().displayingPartIndex = Random.Range(0,2);
			partCount++;

			NetworkServer.Spawn (part);
		}
	}

	public GameObject GetPartPrefabFromID (int partID) {
		return attachedPartsList[partID];
	}

//	[Command]
//	public void CmdPartDestroyed (GameObject part)
//	{
//		partCount--;
//		NetworkServer.Destroy (part);
//		print("Part Destroyed");
//	}

//	[Command]
//	public void CmdPartCollected (GameObject part)
//	{
////		GameObject part = NetworkServer.FindLocalObject (partNetID);
//		NetworkServer.Destroy (part);
//	}
}
