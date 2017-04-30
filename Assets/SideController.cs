using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideController : MonoBehaviour {

	public int index;

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "Attachable") {
			transform.parent.parent.gameObject.GetComponent<PlayerController> ().OnPartHitSide (coll.gameObject, gameObject);
		}
	}

	public bool HasPartAttached () {
		return ( transform.childCount > 0 );
	}
}
