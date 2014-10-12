using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;

[Serializable]
public class MatchManager : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject healthBarPrefab;
	public GameObject matchEndPrefab;
	public GameObject weaponDisplayPrefab;
	public GameObject playerIndicatorPrefab;

	public int nrOfPlayers = 2;
	public List<PlayerController> players;

	bool matchActive = false;


	List<Vector3> spawnPositions;

	// Use this for initialization
	void Start () {
		spawnPositions = new List<Vector3>();
		players = new List<PlayerController>();
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoint");
		foreach(GameObject spawnPoint in spawnPoints) {
			spawnPositions.Add(spawnPoint.transform.position);
		}

		spawnPositions = spawnPositions.OrderBy (o=>o.x).ToList();

		SpawnPlayers ();
		CreateHealthBars ();
		CreateWeaponDisplays ();
		CreatePlayerIndicators ();
		StartMatch ();
	}
	
	// Update is called once per frame
	void Update () {
		if (matchActive) {
			List<PlayerController> playersAlive = new List<PlayerController>();
			foreach (PlayerController player in players) {
				if (player.health > 0) {
					playersAlive.Add (player);
				}
			}

			if(playersAlive.Count == 1) {
				EndMatch ();
				GameObject matchEndObject = Instantiate (matchEndPrefab) as GameObject;
				matchEndObject.transform.FindChild("WinText").GetComponentInChildren<Text> ().text = "Player " + playersAlive[0].playerNumber + " wins!";
				//matchEndObject.GetComponentInChildren<Text> ().text = "Game Over!\nPlayer " + playersAlive[0].playerNumber + " wins!";
			} else if(playersAlive.Count == 0) {
				EndMatch ();
				GameObject matchEndObject = Instantiate (matchEndPrefab) as GameObject;
				matchEndObject.transform.FindChild("WinText").GetComponentInChildren<Text> ().text = "It's a draw!";
			}


		}
	}

	void StartMatch() {
		matchActive = true;
	}

	void EndMatch() {
		matchActive = false;
	}

	void SpawnPlayers() {
		for (int i = 0; i < nrOfPlayers; i++) {
			GameObject playerObject = Instantiate (playerPrefab, spawnPositions[i], Quaternion.identity) as GameObject;
			PlayerController player = playerObject.GetComponent<PlayerController>();
			player.playerNumber = i+1;
			players.Add(player);
		}
	}

	void CreateHealthBars() {
		string nextBarPos = "left";
		foreach(PlayerController player in players) {
			GameObject healthBarObject = Instantiate(healthBarPrefab) as GameObject;
			HealthBar healthBar = healthBarObject.GetComponent<HealthBar>();
			healthBar.player = player;
			healthBar.position = nextBarPos;
			// TODO: Add support for more bars
			nextBarPos = "right";
		}
	}

	void CreateWeaponDisplays() {
		foreach(PlayerController player in players) {
			GameObject weaponDisplayObject = Instantiate(weaponDisplayPrefab) as GameObject;
			WeaponTextDisplay weaponDisplay = weaponDisplayObject.GetComponent<WeaponTextDisplay>();
			weaponDisplay.targetPlayer = player;
		}
	}

	void CreatePlayerIndicators() {
		foreach (PlayerController player in players) {
			GameObject indicatorObject = Instantiate (playerIndicatorPrefab) as GameObject;
			PlayerIndicator indicator = indicatorObject.GetComponent<PlayerIndicator> ();
			indicator.targetPlayer = player;
		}
	}
}
