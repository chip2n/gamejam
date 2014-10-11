using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Hitbox : MonoBehaviour {
	public float damage = 5.0f;
	public Vector3 launchVector;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		BoxCollider2D col = collider2D as BoxCollider2D;

		Gizmos.DrawCube(transform.position, new Vector3(col.size.x, col.size.y, 1));
	}
}
