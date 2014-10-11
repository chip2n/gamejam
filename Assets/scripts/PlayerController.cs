using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerController : MonoBehaviour {
	public float speed = 5.0f;
	Animator animator;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	bool grounded = false;
	public bool controllable = true;
	public GameObject punchHitbox;
	public GameObject explosionPrefab;
	public float health = 100.0f;
	public GameObject deathPrefab;

	public float jumpTime;

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
		animator.SetBool ("grounded", grounded);

		animator.SetBool ("falling", rigidbody2D.velocity.y < 0);
		animator.SetFloat ("vSpeed", rigidbody2D.velocity.y);

		/*
		if (controllable) {
			float val = Input.GetAxis ("Horizontal1");
			transform.position += new Vector3 (val * Time.deltaTime * speed, 0, 0);
			Vector3 rotation = transform.rotation.eulerAngles;
			if (val < 0) {
				rotation.y = 180;
			} else if (val > 0) {
				rotation.y = 0;
			}
			transform.eulerAngles = rotation;

			animator.SetBool ("moving", Math.Abs (val) > 0.5);

			float jump = Input.GetAxis ("Jump1");
			if (jump > 0 && grounded && (Time.time - jumpTime) > 1.0f) {
				Jump ();
			}

			float punch = Input.GetAxis ("Fire1");
			if(punch > 0 && grounded) {
				Punch();
			}
		}
		*/

	}

	public void ProcessHorizontal(float val) {
		if (controllable) {
			transform.position += new Vector3 (val * Time.deltaTime * speed, 0, 0);
			Vector3 rotation = transform.rotation.eulerAngles;
			if (val < 0) {
				rotation.y = 180;
			} else if (val > 0) {
				rotation.y = 0;
			}
			transform.eulerAngles = rotation;
			animator.SetBool ("moving", Math.Abs (val) > 0.5);
		}
	}

	public void ProcessJump(float val) {
		if (controllable) {
			if (val > 0 && grounded && (Time.time - jumpTime) > 1.0f) {
				Jump ();
			}
		}
	}
	
	public void ProcessFire(float val) {
		if (val > 0 && grounded) {
			Punch ();
		}
	}
	
	void Jump() {
		jumpTime = Time.time;
		animator.SetBool("jumping", true);
		rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce (new Vector3 (0, 50000, 0));
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

		if(transform.rotation.y > 0) {
			hitbox.GetComponent<Hitbox>().launchVector = new Vector3(-1,1,0);
		} else {
			hitbox.GetComponent<Hitbox>().launchVector = new Vector3(1,1,0);
		}
		//hitbox.transform.parent = transform;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		//if (coll.gameObject.tag == "Hitbox") {
		Hitbox hitbox = coll.gameObject.GetComponent<Hitbox> ();
		if (hitbox) {
			Debug.Log ("Hit a hitbox.");
			RegisterDamage(hitbox.damage);
			Vector3 launchDir = hitbox.launchVector;
			Debug.Log (launchDir);
			rigidbody2D.AddForce (launchDir * 50000);
		} else {
			Debug.Log ("NOPE");
		}
	}

	void RegisterDamage(float damage) {
		health -= damage;
		if (health <= 0.0f) {
			Vector3 deathPos = new Vector3(transform.position.x, transform.position.y, 0);
			Instantiate (deathPrefab, deathPos, transform.rotation);
			Destroy (gameObject);
		}
	}
}
