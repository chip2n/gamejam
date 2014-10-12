using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Hitbox : MonoBehaviour {
	public float damage = 5.0f;
	public Vector3 launchVector;
	public int owner = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public Vector3 GetLaunchVector(Vector3 pos) {
		if(launchVector.x == 0.0f && launchVector.y == 0.0f) {
			Debug.Log (transform.position + "  " + pos);
			return Vector3.Normalize(transform.position - pos);
		}
		return launchVector;
	}

	public void SetSize(int x, int y) {
		BoxCollider2D col = (BoxCollider2D) collider2D;
		col.size = new Vector2 (x, y);
	}

	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		BoxCollider2D col = collider2D as BoxCollider2D;

		Gizmos.DrawCube(transform.position, new Vector3(col.size.x, col.size.y, 1));
	}
}
