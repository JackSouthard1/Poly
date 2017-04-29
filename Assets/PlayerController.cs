using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour {

	[SyncVar]
	public int playerNumber;

	public GameObject bulletPrefab;
	public Transform bulletSpawn;
	// Use this for initialization

	void Start ()
	{

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
			CmdFire ();
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
		CmdRequestPlayerNumber();
	}

	public override void OnStartClient () {
		SetColor(playerNumber);
	}

	[ClientRpc]
	void RpcPlayerNumberChanged (int newValue) {
		SetColor(newValue);
		print ("Player Numbed Changed");
	}

	void SetColor (int value)
	{
		GetComponent<MeshRenderer>().material.color = GetColorFor(value);
	}

	Color GetColorFor (int playerNumber)
	{
		switch (playerNumber) 
		{
			case 0:
				return Color.red;
				break;
			case 1:
				return Color.blue;
				break;
			case 2:
				return Color.green;
				break;
			default: 
				return Color.black;
				break;
		}
	}

	[Command]
	void CmdRequestPlayerNumber () {
		playerNumber = Random.Range(0,2);
		RpcPlayerNumberChanged(playerNumber);
	}
}
