using UnityEngine;
using System.Collections;

public class BurningStatusEffect : StatusEffect {
	public float duration = 10.0f;
	public float timeBetweenTicks = 2.0f;

	float tickTime;
	float originalDuration;

	// Use this for initialization
	void Start () {
		originalDuration = duration;
	}
	
	// Update is called once per frame
	void Update () {
		PlayerController player = GetComponent<PlayerController> ();
		if (player) {
			if(Time.time - tickTime > timeBetweenTicks) {
				player.SendMessage("RegisterDamage", 5.0f);
				tickTime = Time.time;
			}

		}

		duration = duration - Time.deltaTime;
		if (duration <= 0.0f) {
			Destroy (this);
		}
	}

	public void ResetDuration() {
		duration = originalDuration;
	}
}
