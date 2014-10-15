using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class PlayerController2 : MonoBehaviour {
	public float speed = 5.0f;
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
	
	bool grenade = false;
	
	public float jumpTime;
	public int currentWeapon = 0;



	public float gravity = 9.81f;

	
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		HandleMovement ();
		grounded = IsGrounded ();
		if (grounded) {
			animator.SetBool("jumping", false);
			animator.SetBool("falling", false);
		}
		animator.SetBool ("grounded", grounded);
		
		animator.SetBool ("falling", rigidbody2D.velocity.y < -1.0f);
		animator.SetFloat ("vSpeed", rigidbody2D.velocity.y);
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

	void HandleMovement() {

	}
	
	public void ProcessHorizontal(float val) {
		if (controllable) {
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
	
	public void ProcessJump() {
		if (controllable) {
			
			if (grounded) {
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
		jumpTime = Time.time;
		animator.SetBool("jumping", true);
		//rigidbody2D.isKinematic = false;
		rigidbody2D.AddForce (new Vector3 (0, 1200, 0), ForceMode2D.Impulse);
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
			bool dead = RegisterDamage(hitbox.damage);
			if(dead) {
				GameObject bodyObj = Instantiate (deathPrefab, transform.position, transform.rotation) as GameObject;
				foreach(Transform t in bodyObj.transform) {
					BodyPart bp = t.GetComponent<BodyPart>();
					if(bp) {
						bp.Launch(bp.transform.position - coll.bounds.center);
					}
				}
				Destroy (gameObject);
			} else {
				Vector3 launchDir = hitbox.GetLaunchVector(transform.position);
				rigidbody2D.AddForce (launchDir * hitbox.knockback, ForceMode2D.Impulse);
			}
		}
	}
	
	bool RegisterDamage(float damage) {
		health -= damage;
		if (health <= 0.0f) {
			return true;
		}
		
		return false;
	}
}
