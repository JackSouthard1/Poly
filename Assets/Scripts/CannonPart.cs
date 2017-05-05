using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonPart : AttachedPart {

	private float firerate = 1f;
	private float projectileSpeed = 4f;
	private float projectileLifetime = 4f;
	private string projectileName = "CannonBall";
	public Transform projectileSpawn;
	private PlayerController pc;

	private bool targetInZone = false;

	float fireTimer;

	void Start () {
		fireTimer = firerate;
		pc = transform.parent.parent.parent.GetComponent<PlayerController>();
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
		print ("Cannon Fire");
		Vector3 spawnPos = projectileSpawn.position;
		Quaternion spawnRot = projectileSpawn.rotation;

		pc.Fire (projectileName, spawnPos, spawnRot, projectileSpeed, projectileLifetime);
	}

	public void TargetEnterZone () {
		targetInZone = true;
	}

	public void TargetExitZone () {
		targetInZone = false;
	}
}
