using UnityEngine;
using System.Collections;

public class BodyPart : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter2D(Collider2D coll) {
		//if (coll.gameObject.tag == "Hitbox") {
		Hitbox hitbox = coll.gameObject.GetComponent<Hitbox> ();
		if (hitbox) {
			Launch (hitbox.GetLaunchVector(coll));
		} else {
			Debug.Log ("NOPE");
		}
	}

	public void Launch(Vector3 dir) {
		rigidbody2D.AddForce (dir * 10000);
	}
}
