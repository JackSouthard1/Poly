using UnityEngine.Networking;
using UnityEngine;

public class PartSpawner : NetworkBehaviour {

	public GameObject floatingPartPrefab;

	// list of all avaiable attached part prefabs
	public GameObject[] attachedPartsList;

	private float spawnTimer;
	private float spawnInterval = 2f;
	private int partCount = 0;
	private int partCountMax = 500;

	void Start () {
		spawnTimer = spawnInterval;
	}
	
	void Update ()
	{
		if (isServer) {
			if (spawnTimer <= 0f) {
				spawnTimer = spawnInterval;
				CmdSpawnSegment ();
			} else {
				spawnTimer -= Time.deltaTime;
			}
		}
	}

	[Command]
	public void CmdSpawnSegment ()
	{
		if (isServer) {
			if (partCount < partCountMax) {
				var part = Instantiate (floatingPartPrefab, Vector3.zero, Quaternion.identity) as GameObject;
				part.GetComponent<FloatingPartController> ().displayingPartIndex = 0;
				partCount++;

				NetworkServer.Spawn (part);
			}
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
