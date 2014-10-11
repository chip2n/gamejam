using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class MatchManager : MonoBehaviour {
	public GameObject playerPrefab;
	public GameObject healthBarPrefab;
	public int nrOfPlayers = 2;
	public List<Vector3> spawnPositions;
	public List<PlayerController> players;

	// Use this for initialization
	void Start () {
		spawnPositions = new List<Vector3>();
		players = new List<PlayerController>();
		GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag ("SpawnPoint");
		foreach(GameObject spawnPoint in spawnPoints) {
			spawnPositions.Add(spawnPoint.transform.position);
		}

		SpawnPlayers ();
		CreateHealthBars ();
	}
	
	// Update is called once per frame
	void Update () {
	
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
}
