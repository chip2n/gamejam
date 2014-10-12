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

			if(GetJump()) {
				player.ProcessJump();
			}
			
			float val3 = GetFire();
			player.ProcessFire(val3);

			if(GetWeaponSwitch()) {
				player.SwitchWeapons();
			}
		}
	}

	float GetHorizontal() {
		string axisName = "Horizontal" + player.playerNumber;
		return Input.GetAxis (axisName);
	}

	bool GetJump() {
		string axisName = "Jump" + player.playerNumber;
		return Input.GetButtonDown (axisName);
	}

	float GetFire() {
		string axisName = "Fire" + player.playerNumber;
		return Input.GetAxis (axisName);
	}

	bool GetWeaponSwitch() {
		string axisName = "WeaponSwitch" + player.playerNumber;
		return Input.GetButtonDown (axisName);
	}
}
