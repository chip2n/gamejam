using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerInput : MonoBehaviour {
	PlayerController player;

	// Use this for initialization
	void Start () {
		player = gameObject.GetComponent<PlayerController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (player.controllable) {
			float val = GetHorizontal();
			player.ProcessHorizontal(val);

			float val2 = GetJump();
			player.ProcessJump(val2);
			
			float val3 = GetFire();
			player.ProcessFire(val3);

			float val4 = GetWeaponSwitch();
			player.ProcessWeaponSwitch(val4);
		}
	}

	float GetHorizontal() {
		string axisName = "Horizontal" + player.playerNumber;
		return Input.GetAxis (axisName);
	}

	float GetJump() {
		string axisName = "Jump" + player.playerNumber;
		return Input.GetAxis (axisName);
	}

	float GetFire() {
		string axisName = "Fire" + player.playerNumber;
		return Input.GetAxis (axisName);
	}

	float GetWeaponSwitch() {
		string axisName = "WeaponSwitch" + player.playerNumber;
		return Input.GetAxis (axisName);
	}
}
