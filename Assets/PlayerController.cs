using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!isLocalPlayer) {
			return;
		}

		var x = Input.GetAxis ("Horizontal") * Time.deltaTime * 150f;
		var y = Input.GetAxis ("Vertical") * Time.deltaTime * 3f;

		transform.Rotate (0, 0, x);
		transform.Translate (0, y, 0);

		if (Input.GetKeyDown (KeyCode.Space)) {
			CmdFire();
		}
	}

	[Command]
	void CmdFire () {
		var bullet = (GameObject)Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);

		bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.up * 6;

		NetworkServer.Spawn(bullet);

		Destroy(bullet, 2f);
	}

	public override void OnStartLocalPlayer ()
	{
		GetComponent<MeshRenderer>().material.color = Color.blue;
	}
}
