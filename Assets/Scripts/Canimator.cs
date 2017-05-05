using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canimator : MonoBehaviour {

	public GameObject[] lines;
	private float speed = 3;
	private float multiplier = 1.2f;
	private float minSin = 0.2f;

	void Start () {
//		for (int i = 0; i < transform.childCount; i++) {
//			lines[i] = transform.GetChild(i).gameObject;
//		} 
//		print(lines.Length);
	}
	
	void Update () {
		for (int i = 0; i < lines.Length; i++) {
			float offset = i * 0.5f;
			lines[i].transform.localScale = new Vector3 ((Mathf.Sin((Time.time * speed) + offset) * multiplier) + minSin, lines[i].transform.localScale.y, 1f);
		}
	}
}
