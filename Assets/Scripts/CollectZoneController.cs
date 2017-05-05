using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectZoneController : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag("Collectable")) {
			other.gameObject.GetComponent<SegmentController>().StartTracking(transform.parent);
		}
	}
}
