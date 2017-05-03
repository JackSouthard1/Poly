using System.Collections;
using UnityEngine;

public class AttachedPart : MonoBehaviour {
	private GameObject side;
	public float health;

	void Start () {
		side = transform.parent.gameObject;
	}
}
