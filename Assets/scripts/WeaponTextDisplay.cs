using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[Serializable]
public class WeaponTextDisplay : MonoBehaviour {
	Text text;
	float currentDisplayDuration;
	float displayTime;
	public PlayerController targetPlayer;
	int lastWeapon;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text> ();	
	}
	
	// Update is called once per frame
	void Update () {
		if (targetPlayer) {
			transform.position = new Vector3 (targetPlayer.transform.position.x, targetPlayer.transform.position.y + 0.8f, targetPlayer.transform.position.z);
			if (lastWeapon != targetPlayer.currentWeapon) {
				DisplayWeapon ();
			}
			if (Time.time - displayTime > currentDisplayDuration && text.enabled) {
				HideDisplay ();
			}
		}
	}

	public void DisplayWeapon(float displayDuration = 0.5f) {
		string weaponName = "Unknown";
		if (targetPlayer.currentWeapon == 0) {
			weaponName = "Melee";
		} else if (targetPlayer.currentWeapon == 1) {
			weaponName = "Grenade";
		}
		text.text = weaponName;
		ShowDisplay ();
		currentDisplayDuration = displayDuration;
		displayTime = Time.time;
		lastWeapon = targetPlayer.currentWeapon;
	}

	void HideDisplay() {
		text.enabled = false;
	}

	void ShowDisplay() {
		text.enabled = true;
	}
}
