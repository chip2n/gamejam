using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerController : MonoBehaviour {
	public float speed = 5.0f;
	float fallSpeed = 20.0f;
	Animator animator;
	public Transform groundCheck;
	public float groundRadius;
	public LayerMask whatIsGround;
	bool grounded = false;
	public bool controllable = true;
	public GameObject punchHitbox;
	public GameObject explosionPrefab;
	public float health = 100.0f;
	public GameObject deathPrefab;
	public int playerNumber = 1;
	public GameObject grenadePrefab;
	public GameObject burningStatusPrefab;

	float knockbackTime;

	bool grenade = false;

	public float jumpTime;
	public int currentWeapon = 0;

	Vector2 movementDir;

	int jumpsLeft = 2;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		grounded = IsGrounded ();
		if (grounded) {
			jumpsLeft = 2;
			animator.SetBool("jumping", false);
			animator.SetBool("falling", false);
		}
		animator.SetBool ("grounded", grounded);

		animator.SetBool ("falling", rigidbody2D.velocity.y < -1.0f);
		animator.SetFloat ("vSpeed", rigidbody2D.velocity.y);
	}

	Vector2 lastMovementDir;
	Vector2 lastImpulseDir;


	void FixedUpdate() {
		Vector2 targetSpeed = movementDir * speed;
		Vector2 currentSpeed = rigidbody2D.velocity;
		Vector2 changeDir = targetSpeed - currentSpeed;

		bool changedHorizontalDir = false;
		if (lastMovementDir.x == -movementDir.x) {
			changedHorizontalDir = true;
		}

		if (Math.Abs (rigidbody2D.velocity.x) < 1.0f) {
			lastImpulseDir = new Vector2(0,0);
		}






		if (Math.Abs (rigidbody2D.velocity.x) < speed) {
						if (grounded) {

								rigidbody2D.AddForce (new Vector2 (movementDir.x, 0.0f) * 4000);
								if (lastImpulseDir.x != movementDir.x && movementDir.x != 0.0f) {
										lastImpulseDir = new Vector2 (movementDir.x, 0);
										rigidbody2D.AddForce (new Vector2 (movementDir.x, 0.0f) * 300, ForceMode2D.Impulse);
								}
						} else {

								rigidbody2D.AddForce (new Vector2 (movementDir.x, 0.0f) * 2000);
								//rigidbody2D.AddForce (new Vector2(0.0f, movementDir.y) * 50, ForceMode2D.Impulse);
						}
				}

		/*
		if(Math.Abs (rigidbody2D.velocity.x) > speed) {
			rigidbody2D.velocity = new Vector2(movementDir.x * speed, rigidbody2D.velocity.y);
		}
		*/

		if (movementDir.y == -1.0f) {
			if(rigidbody2D.velocity.y < -fallSpeed - 3.0f) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -fallSpeed - 3.0f);
			}
		} else if (movementDir.y == 0.0f) {
			if(rigidbody2D.velocity.y < -fallSpeed) {
				rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, -fallSpeed);
			}
		}

		SetDirection (movementDir);
		animator.SetBool ("moving", Math.Abs (rigidbody2D.velocity.x) > 0.5);

		if (grounded) {
			Vector3 rotation = transform.rotation.eulerAngles;
			if (movementDir.x < 0.0f) {
				rotation.y = 180;
			} else if (movementDir.x > 0.0f) {
				rotation.y = 0;
			}
			transform.eulerAngles = rotation;
			animator.SetBool ("moving", Math.Abs (rigidbody2D.velocity.x) > 0.5);
		}

		lastMovementDir = movementDir;
	}

	void SetDirection(Vector2 direction) {
		Vector3 rotation = transform.rotation.eulerAngles;
		if (direction.x == 1.0f) {
			rotation.y = 0;
		} else if (direction.x == -1.0f) {
			rotation.y = 180;
		}
		
		transform.eulerAngles = rotation;
	}

	bool IsGrounded() {
		Collider2D[] cols = Physics2D.OverlapCircleAll (groundCheck.position, groundRadius, whatIsGround);
		List<Collider2D> newCols = new List<Collider2D>();


		foreach(Collider2D col in cols) {
			PlayerController pc = col.gameObject.GetComponent<PlayerController>();
			if(pc) {
				if(pc.playerNumber != playerNumber) {
					newCols.Add(col);
				}
			} else {
				newCols.Add(col);
			}
		}

		return newCols.Count > 0;
	}

	public void ProcessHorizontal(float val) {
		if (controllable) {
			movementDir = new Vector2 (val, movementDir.y);
			//Vector3 moveDir = new Vector3 (val * Time.deltaTime * speed, 0, 0);
			//transform.position += moveDir;
			/*
			Vector3 rotation = transform.rotation.eulerAngles;
			if (val < 0) {
					rotation.y = 180;
			} else if (val > 0) {
					rotation.y = 0;
			}
			transform.eulerAngles = rotation;
			animator.SetBool ("moving", Math.Abs (val) > 0.5);
			*/
		}
	}

	public void ProcessVertical(float val) {
		if (controllable) {
			movementDir = new Vector2 (movementDir.x, val);
		}
	}

	public void ProcessJump() {
		if (controllable) {

			if (jumpsLeft > 0) {
				Jump ();
			}
		}
	}
	
	public void ProcessFire(float val) {
		if (val > 0) {
			if(currentWeapon == 0 && grounded) {
				Punch ();
			} else if(currentWeapon == 1) {
				ThrowGrenade();
			}
		}
	}

	public void ProcessWeaponSwitch(float val) {
		if (val > 0) {
			SwitchWeapons ();
		}
	}
	
	void Jump() {
		if (!grounded && jumpsLeft == 2) {
			jumpsLeft = 1;
		}
		if(jumpsLeft > 0) {
			rigidbody2D.velocity = new Vector2(rigidbody2D.velocity.x, 0);
			jumpTime = Time.time;
			animator.SetBool("jumping", true);
			//rigidbody2D.isKinematic = false;
			if(grounded) {
				rigidbody2D.AddForce (new Vector3 (0, 1500, 0), ForceMode2D.Impulse);
			} else {
				rigidbody2D.AddForce (new Vector3 (0, 1200, 0), ForceMode2D.Impulse);
			}
			jumpsLeft = jumpsLeft - 1;
		}
	}

	void Punch() {
		controllable = false;
		animator.SetBool("punching", true);
	}

	void ThrowGrenade() {
		if (!grenade) {
			grenade = true;
			animator.SetBool ("grenade", grenade);
			Vector3 grenadeSpawnPoint;
			Vector3 grenadeLaunchVector;

			if(transform.rotation.y > 0) {
				grenadeSpawnPoint = new Vector3 (transform.position.x - 0.5f, transform.position.y, -4);
				grenadeLaunchVector = new Vector2 (-0.5f, 0.5f) * 800;
			} else {
				grenadeSpawnPoint = new Vector3 (transform.position.x + 0.2f, transform.position.y, -4);
				grenadeLaunchVector = new Vector2 (0.5f, 0.5f) * 800;

			}
			GameObject grenadeObject = Instantiate (grenadePrefab, grenadeSpawnPoint, Quaternion.identity) as GameObject;
			grenadeObject.rigidbody2D.AddForce (grenadeLaunchVector);
			int torque = UnityEngine.Random.Range(-100, 100);
			grenadeObject.rigidbody2D.AddTorque(torque);
			//animator.SetBool("punching", true);
			// Grenade delay
			Invoke ("OnGrenadeThrowFinished", 0.4f);
		}
	}

	public void SwitchWeapons() {
		currentWeapon = (currentWeapon + 1) % 2;
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

		hitbox.GetComponent<Hitbox>().owner = playerNumber;
		//hitbox.transform.parent = transform;
	}

	void OnGrenadeThrowFinishedAnim() {
		animator.SetBool ("grenade", grenade);
	}

	void OnGrenadeThrowFinished() {
		grenade = false;
		animator.SetBool ("grenade", grenade);
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Hitbox hitbox = coll.gameObject.GetComponent<Hitbox> ();
		if (hitbox && hitbox.owner != playerNumber) {
			bool dead = RegisterDamage(hitbox.damage, true, coll.bounds.center);
			if(!dead) {
				Vector3 launchDir = hitbox.GetLaunchVector(transform.position);
				Launch(launchDir, hitbox.knockback);
			}
		}
	}

	void Launch(Vector2 launchDir, float knockback) {
		Debug.Log ("Launching with knock back: " + knockback + " in (" + launchDir.x + "," + launchDir.y + ")");
		rigidbody2D.AddForce (launchDir * knockback, ForceMode2D.Impulse);
		knockbackTime = Time.time;
	}

	bool RegisterDamage(float damage) {
		return RegisterDamage (damage, false, Vector3.zero);
	}

	bool RegisterDamage(float damage, bool createBodyparts, Vector3 impactPoint) {
		health -= damage;
		if (health <= 0.0f) {
			if(createBodyparts) {
				Debug.Log ("WOUIE");
				GameObject bodyObj = Instantiate (deathPrefab, transform.position, transform.rotation) as GameObject;
				foreach(Transform t in bodyObj.transform) {
					BodyPart bp = t.GetComponent<BodyPart>();
					if(bp) {
						bp.Launch(bp.transform.position - impactPoint);
					}
				}
			}
			Destroy (gameObject);
			return true;
		}

		return false;
	}
}
