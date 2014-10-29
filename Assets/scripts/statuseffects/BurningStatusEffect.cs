using UnityEngine;
using System.Collections;

public class BurningStatusEffect : StatusEffect {
	PlayerController player;
	public float duration = 3.0f;
	public float timeLeft;
	public float timeBetweenTicks = 1.0f;

	float tickTime;

	GameObject fire;

	// Use this for initialization
	void Start () {
		timeLeft = duration;
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
		timeLeft = timeLeft - Time.deltaTime;
		if (timeLeft <= 0.0f) {
			Destroy (fire);
			Destroy (this);
		}
	}

	public void ResetDuration() {
		timeLeft = duration;
	}
}
