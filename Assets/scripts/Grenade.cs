using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Grenade : MonoBehaviour {
	public float timer = 3.0f;
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
			Hitbox hitbox = explosion.GetComponent<Hitbox>();
			hitbox.damage = damage;
			hitbox.SetSize(5, 5);
			Destroy (gameObject);
		}
	}
}
