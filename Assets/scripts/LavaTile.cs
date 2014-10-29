using UnityEngine;
using System.Collections;

public class LavaTile : Tile {

	// Use this for initialization
	void Start () {
		BoxCollider2D col = GetComponent<BoxCollider2D> ();
		col.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col) {
		Debug.Log ("Collided with lava yo");
		PlayerController player = col.gameObject.GetComponent<PlayerController> ();

		if (player) {
			BurningStatusEffect s = player.GetComponent<BurningStatusEffect>();
			if(s != null) {
				s.ResetDuration();
			} else {
				player.gameObject.AddComponent<BurningStatusEffect>();
			}
		}
	}
}
