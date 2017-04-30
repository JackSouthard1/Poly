﻿using UnityEngine.Networking;
using UnityEngine;

public class SegmentSpawner : NetworkBehaviour {

	public GameObject segmentPrefab;
//	private GameObject segments;

	private float spawnTimer;
	private float spawnInterval = 1f;
	private int segmentCount = 0;
	private int segmentCountMax = 100;
	// Use this for initialization
	void Start () {
		spawnTimer = spawnInterval;
//		segments = GameObject.Find("Segments");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (isServer) {
			if (spawnTimer <= 0f) {
				spawnTimer = spawnInterval;
				SpawnSegment ();
			} else {
				spawnTimer -= Time.deltaTime;
			}
		}
	}
	void SpawnSegment ()
	{
		if (isServer) {
			if (segmentCount < segmentCountMax) {
				var segment = Instantiate (segmentPrefab, Vector3.zero, Quaternion.identity) as GameObject;
//				segment.transform.parent = this.gameObject.transform;
				segmentCount++;

				NetworkServer.Spawn (segment);
			}
		}
	}
	[Command]
	public void CmdCollectSegment (GameObject segment)
	{
		segmentCount--;
		NetworkServer.Destroy (segment);
	}
}
