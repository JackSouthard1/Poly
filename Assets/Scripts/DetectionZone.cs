using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour {

	CannonPart cp;

	void Start () {
		cp = transform.parent.gameObject.GetComponent<CannonPart>();

	}
	void OnTriggerEnter2D (Collider2D other)
	{
		
	}

	void OnTriggerExit2D (Collider2D other) 
	{

	}
}
