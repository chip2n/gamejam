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
			float val2 = GetVertical();
			player.ProcessVertical(val2);

			if(GetJump()) {
				player.ProcessJump();
			}
			
			float val3 = GetFire();
			player.ProcessFire(val3);

			if(GetWeaponSwitch()) {
				player.SwitchWeapons();
			}

			if(Input.GetAxis("Exit") > 0) {
				Application.Quit();
			}
		}
	}

	float GetHorizontal() {
		string axisName = "Horizontal" + player.playerNumber;
		return Input.GetAxisRaw (axisName);
	}

	float GetVertical() {
		string axisName = "Vertical" + player.playerNumber;
		return Input.GetAxisRaw (axisName);
	}

	bool GetJump() {
		string axisName = "Jump" + player.playerNumber;
		return Input.GetButtonDown (axisName);
	}
	//TODO: use GetAxisRaw
	float GetFire() {
		string axisName = "Fire" + player.playerNumber;
		return Input.GetAxis (axisName);
	}

	bool GetWeaponSwitch() {
		string axisName = "WeaponSwitch" + player.playerNumber;
		return Input.GetButtonDown (axisName);
	}
}
