using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int width = 16;
	public int height = 16;
	public bool collidable = false;
	public EdgeCollider2D leftCollider;
	public EdgeCollider2D rightCollider;
	public EdgeCollider2D topCollider;
	public EdgeCollider2D bottomCollider;

	// Use this for initialization
	void Start () {
		leftCollider = transform.FindChild ("left").GetComponent<EdgeCollider2D>();
		rightCollider = transform.FindChild ("right").GetComponent<EdgeCollider2D>();
		topCollider = transform.FindChild ("top").GetComponent<EdgeCollider2D>();
		bottomCollider = transform.FindChild ("bottom").GetComponent<EdgeCollider2D>();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void SetPosition(int x, int y, float z = 0.0f) {
		transform.position = new Vector3((float) (x * 1.6f), (float) (y * 1.6f), z);
	}

	public void SetCollidable(bool val) {
		collidable = val;
		//collider2D.enabled = val;
		leftCollider.enabled = val;
		rightCollider.enabled = val;
		topCollider.enabled = val;
		bottomCollider.enabled = val;
		if (val) {
			SetLayer(9);
		} else {
			SetLayer(0);
		}
	}

	public void SetLayer(int layerNr) {;
		gameObject.layer = layerNr;
		leftCollider.gameObject.layer = layerNr;
		rightCollider.gameObject.layer = layerNr;
		topCollider.gameObject.layer = layerNr;
		bottomCollider.gameObject.layer = layerNr;
	}
}
