using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class HealthBar : MonoBehaviour {
	public GameObject fullHeartPrefab;
	public GameObject halfHeartPrefab;
	public float heartHealth = 10.0f;
	Transform canvas;
	public PlayerController player;
	public float lastHealth = 0.0f;

	// Use this for initialization
	void Start () {
		canvas = transform.Find("Canvas");
		UpdateHealthBar ();
	}
	
	// Update is called once per frame
	void Update () {
		if (lastHealth != player.health) {
			UpdateHealthBar ();
		}
	}

	public void RegisterDamage(float damage) {
		player.health -= damage;
		UpdateHealthBar();
	}

	void UpdateHealthBar() {
		foreach(Transform c in canvas) {
			Destroy (c.gameObject);
		}

		int nrOfFullHearts = CalculateNumberOfFullHearts();
		int nrOfHalfHearts = CalculateNumberOfHalfHearts();
		// Instantiate full hearts
		for (int i = 0; i < nrOfFullHearts; i++) {
			GameObject fullHeart = Instantiate(fullHeartPrefab) as GameObject;
			fullHeart.transform.parent = canvas;
			fullHeart.transform.Translate(new Vector3(i*10,0,0));
		}
		for (int i = 0; i < nrOfHalfHearts; i++) {
			GameObject halfHeart = Instantiate(halfHeartPrefab) as GameObject;
			halfHeart.transform.parent = canvas;
			halfHeart.transform.Translate(new Vector3((i + nrOfFullHearts)*10,0,0));
		}

		lastHealth = player.health;

	}

	float CalculateNumberOfHearts() {
		float nrOfHearts = player.health / heartHealth;
		//nrOfHearts = (float) Math.Round (nrOfHearts * 2.0f) * 0.5f;
		return nrOfHearts;
	}

	int CalculateNumberOfFullHearts() {
		return (int) CalculateNumberOfHearts();
	}

	int CalculateNumberOfHalfHearts() {
		float nrOfHearts = CalculateNumberOfHearts();
		float r = (nrOfHearts - ((float)CalculateNumberOfFullHearts())) * 2;
		return (int)r;
	}
}
