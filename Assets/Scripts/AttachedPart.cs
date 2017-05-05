using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class AttachedPart : MonoBehaviour {
	private GameObject side;
	public float health;

	public bool isLocalPart;

	void Awake () {
		side = transform.parent.gameObject;
		isLocalPart = transform.parent.parent.parent.gameObject.GetComponent<NetworkIdentity>().isLocalPlayer;
	}
}
