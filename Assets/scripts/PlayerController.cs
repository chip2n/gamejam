using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerController : MonoBehaviour {
	float speed = 5.0f;
	Animator animator;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	bool grounded = false;
	public bool controllable = true;
	public GameObject punchHitbox;
	public GameObject explosionPrefab;

	float jumpTime;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		if (grounded) {
			animator.SetBool("jumping", false);
			animator.SetBool("falling", false);
		}
		Debug.Log (grounded);
		animator.SetBool ("grounded", grounded);

		animator.SetBool ("falling", rigidbody2D.velocity.y < 0);
		animator.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		if (controllable) {
			float val = Input.GetAxis ("Horizontal");
			transform.position += new Vector3 (val * Time.deltaTime * speed, 0, 0);
			Vector3 rotation = transform.rotation.eulerAngles;
			if (val < 0) {
				rotation.y = 180;
			} else if (val > 0) {
				rotation.y = 0;
			}
			transform.eulerAngles = rotation;

			animator.SetBool ("moving", Math.Abs (val) > 0.5);

			float jump = Input.GetAxis ("Jump");
			if (jump > 0 && grounded) {
				Jump ();
			}

			float punch = Input.GetAxis ("Fire1");
			if(punch > 0 && grounded) {
				Punch();
			}
		}

	}

	void Jump() {
		jumpTime = Time.time;
		animator.SetBool("jumping", true);
		rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce (new Vector3 (0, 4500, 0));
	}

	void Punch() {
		controllable = false;
		animator.SetBool("punching", true);
	}

	void OnFinishedAttack() {
		controllable = true;
		animator.SetBool("punching", false);
		/*
		foreach(Transform child in transform) {
			if(child.name.StartsWith("Hitbox")) {
				Destroy (child.gameObject);
			}
		}
		*/
	}

	void OnCreateExplosion() {
		float offset = 1.0f;
		if (transform.rotation.eulerAngles.y > 0) {
			offset = -1.0f;
		}
		Vector3 explosionSpawnPoint = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z + 0.1f);
		GameObject hitbox = Instantiate (explosionPrefab, explosionSpawnPoint, transform.rotation) as GameObject;
		hitbox.transform.parent = transform;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Hitbox") {
			Debug.Log ("Hit a hitbox.");
		}
	}
}
