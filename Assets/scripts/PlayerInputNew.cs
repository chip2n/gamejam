using UnityEngine;
using System.Collections;

public class PlayerInputNew : MonoBehaviour {
	
	public struct PlayerInput {
		public Vector2 movementDirection;
		public bool jump;
		public bool fire;
		public bool weaponSwitch;
	}

	PlayerControllerNew player;

	// Use this for initialization
	void Start () {
		player = gameObject.GetComponent<PlayerControllerNew> ();
	}
	
	// Update is called once per frame
	void Update () {
		PlayerInput input;
		input.movementDirection = new Vector2(GetHorizontal(), GetVertical());
		input.jump = GetJump();
		input.fire = GetFire();
		input.weaponSwitch = GetWeaponSwitch();

		player.SendMessage ("ProcessInput", input);
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
		return Input.GetButton (axisName);
	}

	bool GetFire() {
		string axisName = "Fire" + player.playerNumber;
		return Input.GetButton (axisName);
	}
	
	bool GetWeaponSwitch() {
		string axisName = "WeaponSwitch" + player.playerNumber;
		return Input.GetButtonDown (axisName);
	}
}
