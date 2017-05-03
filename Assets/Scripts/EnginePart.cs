using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnginePart : AttachedPart {

	private float thrust;
	private float angleMax = 30f;
	private bool powered = false;
	private ParticleSystem ps;
	private float goalEmissionRate = 5f;
	private Rigidbody2D playerRB;

	void Start () {
		playerRB = transform.parent.parent.parent.GetComponent<Rigidbody2D>();
		ps = transform.GetChild(0).GetComponent<ParticleSystem>();

		TurnOn();
	}

	void Update ()
	{
		if (ShouldThrust()) {
			if (!powered) {
				TurnOn();
			}
		} else if (powered) {
			TurnOff();
		}
	}

	void TurnOn () {
		powered = true;
		ps.Play();
	}

	void TurnOff () {
		powered = false;
		ps.Stop();
	}

	bool ShouldThrust ()
	{
		Vector2 playerVelocity = playerRB.velocity;
		Quaternion thrusterWorldRotation = transform.rotation;

		float thrusterRoationAngle360 = thrusterWorldRotation.eulerAngles.z;
		float thrusterRotationAngle = GetRotationFrom360Angle(thrusterRoationAngle360);
		float playerRotationAngle = Mathf.Atan2 (playerVelocity.y, playerVelocity.x) * Mathf.Rad2Deg;

		print ("Player Angle: " + playerRotationAngle + " Thruster Angle: " + thrusterRotationAngle);

		if (Mathf.Abs (Mathf.Abs(thrusterRotationAngle) - Mathf.Abs(playerRotationAngle)) <= angleMax) {
			return true;
		} else {
			return false;
		}
	}

	float GetRotationFrom360Angle (float angle360)
	{
		angle360 -= 90;
		if (angle360 > 180) {
			angle360 = -(360 - angle360);
		}
		return angle360;
	}
}
