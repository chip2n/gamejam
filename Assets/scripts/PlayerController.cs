using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerController : MonoBehaviour {
	float speed = 5.0f;
	Animator animator;

	float jumpTime;
	bool inAir = false;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		float val = Input.GetAxis ("Horizontal");
		transform.position += new Vector3 (val * Time.deltaTime * speed, 0, 0);
		Vector3 rotation = transform.rotation.eulerAngles;
		if (val < 0) {
			rotation.y = 180;
		} else {
			rotation.y = 0;
		}
		transform.eulerAngles = rotation;

		animator.SetBool("moving", Math.Abs(val) > 0.5);

		float jump = Input.GetAxis("Jump");
		if(jump > 0 && !inAir) {
			Jump();
		}

	}

	void Jump() {
		jumpTime = Time.time;
		animator.SetBool("jumping", true);
		rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce (new Vector3 (0, 300, 0));
		inAir = true;
	}

	void OnCollisionEnter3D(Collision2D coll) {
		if(coll.gameObject.tag == "tile") {
			Debug.Log ("Collided with tile.");
		} else {
			Debug.Log ("Collided with something else :SS");
		}
	}
}
