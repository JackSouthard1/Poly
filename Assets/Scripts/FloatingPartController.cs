using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class FloatingPartController : NetworkBehaviour {

	public Sprite[] partSprites;

	[SyncVar(hook="DisplayPart")]
	public int displayingPartIndex;

	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find("Parts Container").transform;
		DisplayPart(displayingPartIndex);
	}

	public void DisplayPart (int partIndex) {
		GetComponent<SpriteRenderer>().sprite = partSprites[partIndex];
	}

	public int GetPartIndex () {
		return displayingPartIndex;
	}





//
//	public bool isAttached () {
//		return (state == State.Attached);
//	}
//
//	public void Attach (GameObject side)
//	{
//		if (side == null) {
//			print ("No Valid Side");
//			return;
//		}
//		state = State.Attached;
//
////		gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
//		if (gameObject.GetComponent<Rigidbody2D> () == null) {
//			print("No Rigidbody2D");
//		} else {
//			Destroy(gameObject.GetComponent<Rigidbody2D>());
//		}
//
//		gameObject.transform.localPosition = Vector3.zero;
//		gameObject.transform.localRotation = Quaternion.identity;
//		gameObject.transform.SetParent(side.transform, false);
//	}
//
//	public void Detach () {
//		if (isAttached()) {
//			state = State.Detached;
//			Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
//			rb.gravityScale = 0f;
//			transform.SetParent(GameObject.Find("PartsManager").transform, true);
//			rb.AddForce(transform.up * detachForce);
//		}
//	}
}
