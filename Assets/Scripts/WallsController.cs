using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallsController : MonoBehaviour {

	public static int mapSize = 50;
	private static float wallThickness = 0.1f;
	public static float safeSpawnDistance = mapSize - (wallThickness * 5);

	public GameObject top;
	public GameObject right;
	public GameObject bottom;
	public GameObject left;

	// Use this for initialization
	void Awake () {
		PositionWalls();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void PositionWalls ()
	{
		float halfSize = mapSize/2;

		// position
		top.transform.position = 	new Vector3(0, halfSize, 0);
		right.transform.position = 	new Vector3(halfSize, 0, 0);
		bottom.transform.position = new Vector3(0, -halfSize, 0);
		left.transform.position = 	new Vector3(-halfSize, 0, 0);

		// scale
		Vector3 scale = new Vector3(mapSize + wallThickness, wallThickness, 1);
		top.transform.localScale = scale;
		right.transform.localScale = scale;
		bottom.transform.localScale = scale;
		left.transform.localScale = scale;
	}
}
