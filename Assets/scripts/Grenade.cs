using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Grenade : MonoBehaviour {
	public float timer = 2.0f;
	public GameObject explosionPrefab;
	public float damage = 100.0f;
	 
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if (timer <= 0) {
			Vector3 explosionSpawnPoint = new Vector3(transform.position.x, transform.position.y, -2.0f);
			GameObject explosion = Instantiate (explosionPrefab, explosionSpawnPoint, transform.rotation) as GameObject;
			// TODO: Add custom grenade explosion animation instead of scaling the small one
			explosion.transform.localScale = explosion.transform.localScale * 3.0f;
			Hitbox hitbox = explosion.GetComponent<Hitbox>();
			hitbox.damage = damage;
			hitbox.knockback = 2000;
			hitbox.SetSize(2, 2);
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Hitbox hitbox = coll.gameObject.GetComponent<Hitbox> ();
		Vector3 launchDir = hitbox.GetLaunchVector(transform.position);
		rigidbody2D.AddForce (launchDir * hitbox.knockback / 35, ForceMode2D.Impulse);
	}
}
