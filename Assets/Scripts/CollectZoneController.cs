using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectZoneController : MonoBehaviour {

	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.CompareTag("Collectable")) {
			other.gameObject.GetComponent<SegmentController>().StartTracking(gameObject.transform);
		}
	}
}
