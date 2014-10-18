using UnityEngine;
using System.Collections;

public class BurningStatusEffect : StatusEffect {
	PlayerController player;
	public float duration = 10.0f;
	public float timeBetweenTicks = 2.0f;

	float tickTime;
	float originalDuration;

	GameObject fire;

	// Use this for initialization
	void Start () {
		originalDuration = duration;
		player = GetComponent<PlayerController> ();
		fire = Instantiate(player.burningStatusPrefab) as GameObject;
		fire.transform.position = player.transform.position;
		fire.transform.parent = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (player) {
			if(Time.time - tickTime > timeBetweenTicks) {
				player.SendMessage("RegisterDamage", 5.0f);
				tickTime = Time.time;
			}

		}

		duration = duration - Time.deltaTime;
		if (duration <= 0.0f) {
			Destroy (fire);
			Destroy (this);
		}
	}

	public void ResetDuration() {
		duration = originalDuration;
	}
}
