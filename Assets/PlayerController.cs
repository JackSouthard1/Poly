using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	[SyncVar]
	public int playerNumber;

	private float sidesCountMin = 2.0f;
	private int sidesCountMax = 12;
	private float sidesCountIncrement = 0.05f;
	private float[] angles;
	private float radius;

	private Color[] playerColors = new Color[] {
		new Color(0.30f, 0.63f, 0.73f),
		new Color(0.85f, 0.63f, 0.28f),
		new Color(0.77f, 0.32f, 0.38f),
		new Color(0.59f, 0.45f, 0.82f),
		new Color(0.33f, 0.72f, 0.33f)
	};

	// side prefabs
	private GameObject[] sidesGOArray;
	public GameObject sidePrefab;
	public GameObject sidesContainer;

	[SyncVar(hook="SetSidesCount")]
	private float sidesCount;

	void Start ()
	{
		SetupRendering();
	}

	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		if (Input.GetKey ("=")) {
			CmdChangeSidesCount (sidesCount + sidesCountIncrement);
		}
		if (Input.GetKey ("-")) {
			CmdChangeSidesCount (sidesCount - sidesCountIncrement);
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150f;
		var y = Input.GetAxis ("Vertical") * Time.deltaTime * 3f;

		transform.Rotate (0, 0, -x);
		transform.Translate (0, y, 0);
	}

	public override void OnStartLocalPlayer ()
	{
		CmdRequestPlayerNumber();
		CmdChangeSidesCount(2.2f);
	}

	public override void OnStartClient ()
	{
		if (!isLocalPlayer) {
			SetColor (playerNumber);
			SetSidesCount(sidesCount);
		}
	}

	// Networking ***********

	[Command]
	void CmdRequestPlayerNumber () {
		playerNumber = Random.Range(0,4);
		RpcPlayerNumberChanged(playerNumber);
	}

	[Command]
	void CmdChangeSidesCount (float newValue) {
		sidesCount = newValue;
		RpcSidesCountChanged(sidesCount);
	}

	[ClientRpc]
	void RpcPlayerNumberChanged (int newValue) {
		SetColor(newValue);
	}

	[ClientRpc]
	void RpcSidesCountChanged (float newValue) {
		SetSidesCount(newValue);
	}

	void SetColor (int value)
	{
		GetComponent<MeshRenderer>().material.color = playerColors[value];
	}

	// RENDERING ***************************************************************************************************************************************************

	void SetupRendering ()
	{
		MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();

		gameObject.AddComponent (typeof(PolygonCollider2D));

		// setup pool of sides in the sidesContainer
		sidesGOArray = new GameObject[sidesCountMax];
		for (var i = 0; i < sidesCountMax; i++) {
			var newSide = Instantiate(sidePrefab, Vector3.zero, Quaternion.identity) as GameObject;
			newSide.transform.parent = sidesContainer.transform;
			newSide.SetActive(false);
			sidesGOArray[i] = newSide;
		}
	}

	void SetSidesCount (float newValue)
	{
//		if (sides == newValue) {
//			return;
//		}
		if (newValue < sidesCountMin) {
			newValue = sidesCountMin;
		} else if (newValue > sidesCountMax) {
			newValue = sidesCountMax;
		}

		sidesCount = newValue;
		UpdateRendering();
//		gameObject.GetComponent<CircleCollider2D>().radius = radius + 0.75f;
	}

	// Update mesh and polygon collider
	void UpdateRendering ()
	{
		float[] angles = CalculateAngles (sidesCount);
		int anglesCount = angles.Length;
		int vertexCount = anglesCount + 1;  //includes center
		//for every angle there is own vertex; along the unit circle

		// assume that difference between first and second angles is otherAngle
		radius = PolyRadiusFromSidesCount (angles [1] - angles [0]);

		// Setup temporary arrays
		var vertices = new Vector3[vertexCount]; 
		var uv = new Vector2[vertexCount];

		// Create a vertex for center of polygon
		var center = new Vector3 (0, 0, 0);
		var centerIndex = 0;
		vertices [centerIndex] = center; 
		uv [centerIndex] = new Vector2 (0, 0);

		// For each angle, create a vertex and matching UV texture mapping information
		// V1-Vn are the outside vertices in clockwise order, V1 is top at 0 degrees
		for (var i = 0; i < anglesCount; i++) {
			var angle = angles [i];

			// Vertex
			var x = Mathf.Cos (angle * Mathf.Deg2Rad) * radius;
			var y = Mathf.Sin (angle * Mathf.Deg2Rad) * radius;
			vertices [i + 1] = new Vector3 (x, y, 0);

			// UV: Read about texture mapping here: https://en.wikipedia.org/wiki/UV_mapping
			var u = Mathf.Cos (angle * Mathf.Deg2Rad); // Does this work?
			var v = Mathf.Sin (angle * Mathf.Deg2Rad); // Does this work?
			uv [i + 1] = new Vector2 (u, v);

		}

		// For each vertex, not including center (V0), create a triangle in
		// the mesh from center to next vertex to it (counterclockwise)
		var triangles = new int[anglesCount * 3];
		// iterate through vertices
		for (var i = 0; i < anglesCount; i++) {

			triangles [i * 3 + 0] = centerIndex;
			triangles [i * 3 + 1] = (i == anglesCount - 1) ? 1 : i + 2; // in last case, use 0 degrees (V1);
			triangles [i * 3 + 2] = i + 1;

		}

		// UPDATE MESH and FILTER
		Mesh m = new Mesh ();
		m.vertices = vertices;
		m.uv = uv;
		m.triangles = triangles;
		m.RecalculateBounds ();
		m.RecalculateNormals ();

		var mf = gameObject.GetComponent<MeshFilter> ();
		mf.mesh = m;

		var mr = gameObject.GetComponent<MeshRenderer>();

		// UPDATE POLYGON COLLIDER
		// the points are all the verticies, minus V0 (center vertex of mesh)

		// build array of points for poly collider
		var points = new Vector2[anglesCount];
		for (var i = 0; i < anglesCount; i++) {
			var vertex = vertices [i + 1];
			points [i] = new Vector2 (vertex.x, vertex.y);
		}

		// set path
		var pc = gameObject.GetComponent<PolygonCollider2D> ();
		pc.SetPath (0, points);

		var midpointAngles = new float[anglesCount];
		var midpointDistances = new float[anglesCount];

		for (var i = 0; i < anglesCount; i++) {
			var a = angles [i];
			// if last angle, use first angle; otherwise use next angle
			var b = (i == anglesCount - 1) ? angles [0] : angles [i + 1]; 

			// if b is less than a then add 360 to b
			if (a > b) b += 360;
		
			// store what angle the side midpoint is at
			midpointAngles[i] = (a + b) / 2;
			// store distance from center
			midpointDistances[i] = DistanceFromCenterForSideWithArcAngle(b-a);
		}

		// activate and place sides
		for (var i = 0; i < sidesGOArray.Length; i++) {
			var side = sidesGOArray [i];
			if (i+1 < anglesCount) {
				// make active
				side.SetActive (true);
				// set transform
				var angle = midpointAngles[i];
				var distance = midpointDistances[i];
				var x = Mathf.Cos(angle * Mathf.Deg2Rad) * distance;
				var y = Mathf.Sin(angle * Mathf.Deg2Rad) * distance;
				side.transform.localPosition = new Vector3 (x, y, 0);
				side.transform.localRotation = Quaternion.Euler (0, 0, angle-90);
			} else {
//				// make inactive, but first check if it has an attached child part
//				foreach (Transform t in side.GetComponentInChildren<Transform>()) {
//					DetachPart(t.gameObject);
//				}
				side.SetActive(false);
			}
		}
	}

	// returns angles of all verticies like [0, 120, 240] for triangle
	float[] CalculateAngles (float sides)
	{
		int vertices = Mathf.CeilToInt(sides);

		// special case when shape is complete like sides = 4.0
		bool isEqualSides = (sides == vertices);

		// for sides = 3.1, grow = .1 and grow is the growing side
		float grow = sides - Mathf.Floor (sides);
		float growAngleMax = 360f / sides;
		float growAngle = growAngleMax * grow;
		float anglesOffset = 0f;
//		float anglesOffset = growAngle / 2;
//		float anglesOffset = growAngle / 2 + ((Mathf.Floor(sides) % 2) * 180);

		// gives all other angles rest of the circle
		float otherAnglesTotal = 360f - growAngle;
		int otherVerticesCount = vertices;
		if (!isEqualSides) {
			otherVerticesCount -= 1;
		}
//		print (otherVerticesCount);

		float otherAngle = otherAnglesTotal / otherVerticesCount;

		// builds array of angles to construct polygon
		var angles = new float[vertices];

		for (var i = 0; i < otherVerticesCount; i++) {
			angles [i] = (i * otherAngle) + anglesOffset;
		}

		if (!isEqualSides) {
			angles[otherVerticesCount] = (360f - growAngle) + anglesOffset;
		}

//		print(ArrayToString(angles) + "growAngle:" + growAngle.ToString("0"));

		return angles;
	}

	// HELPERS ******************************************8

	float PolyRadiusFromSidesCount(float otherAngle) {
		return 1/(2 * Mathf.Sin(otherAngle * Mathf.Deg2Rad/2));
	}

	float DistanceFromCenterForSideWithArcAngle(float arcAngle) {
		return 1/(2 * Mathf.Tan(arcAngle * Mathf.Deg2Rad/2));
	}

	string ArrayToString (float[] array) {
		var s = "";
		for (var i = 0; i < array.Length; i++) {
			s += array[i].ToString("0");
			s += ",";
		}

		return s;
	}
}
