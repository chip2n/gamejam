using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	public int width = 16;
	public int height = 16;
	public bool collidable = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void setPosition(int x, int y, float z = 0.0f) {
		transform.position = new Vector3((float) (x * 1.6f), (float) (y * 1.6f), z);
	}
}
