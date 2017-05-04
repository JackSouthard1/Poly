using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CannonPart : AttachedPart {

	private float firerate = 1f;
	private float projectileSpeed = 5f;
	private float projectileLifetime = 3f;
	public GameObject cannonBall;
	public Transform projectileSpawn;
	private PartSpawner ps;

	private bool targetInZone = true;

	float fireTimer;

	void Start () {
		fireTimer = firerate;
		ps = GameObject.Find("Part Spawner").GetComponent<PartSpawner>();
	}

	void Update ()
	{
		if (isLocalPart) {
			if (fireTimer <= 0f) {
				fireTimer = firerate;
				if (targetInZone) {
					Fire();
				}
			} else {
				fireTimer -= Time.deltaTime;
			}
		}
	}

	void Fire () {
		Vector3 spawnPos = projectileSpawn.position;
		Quaternion spawnRot = projectileSpawn.rotation;

		ps.CmdFire (cannonBall, spawnPos, spawnRot, projectileSpeed, projectileLifetime);
	}

	public void TargetEnterZone () {
		targetInZone = true;
	}

	public void TargetExitZone () {
//		targetInZone = false;
	}
}
