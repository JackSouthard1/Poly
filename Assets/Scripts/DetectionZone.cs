using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour {

	private CannonPart cp;

	void Start () {
		cp = transform.parent.gameObject.GetComponent<CannonPart>();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag("Player")) {
			cp.TargetEnterZone();
		}
	}

	void OnTriggerExit2D (Collider2D other) 
	{
		if (other.gameObject.CompareTag("Player")) {
			cp.TargetExitZone();
		}
	}
}
