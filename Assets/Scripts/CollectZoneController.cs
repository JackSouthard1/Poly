using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectZoneController : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D other) {
		print("Attract");

		if (other.gameObject.CompareTag("Collectable")) {
			other.gameObject.GetComponent<SegmentController>().StartTracking(transform.parent);
		}
	}
}
