using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D coll) {
		//if (coll.gameObject.tag == "Hitbox") {
		Hitbox hitbox = coll.gameObject.GetComponent<Hitbox> ();
		if (hitbox) {
			Debug.Log ("Hit a hitbox.");
			Vector3 launchDir = hitbox.launchVector;
			Debug.Log (launchDir);
			rigidbody2D.AddForce (launchDir * 10000);
		} else {
			Debug.Log ("NOPE");
		}
	}
}
