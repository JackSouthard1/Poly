using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentController : MonoBehaviour {

	private Transform target;
	private float attractSpeed = 0.5f;

	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("Segments Container").transform;
	}

	public void StartTracking (Transform other)
	{
		target = other;
	}

	void Update ()
	{
		if (target != null) {
			gameObject.GetComponent<Rigidbody2D>().AddForce((target.position - gameObject.transform.position) * attractSpeed);
		}
	}
}
