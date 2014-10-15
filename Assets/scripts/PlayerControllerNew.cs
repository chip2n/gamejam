using UnityEngine;
using System.Collections;

public class PlayerControllerNew : MonoBehaviour {
	public bool controllable = true;
	public Vector2 movementDir;
	public float speed = 5.0f;
	public int playerNumber = 1;
	public float gravity = 9.81f;
	public Vector2 velocity;
	public Transform groundCheck;

	public enum State { Grounded, Falling, Attacking };
	public State state;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(velocity * Time.deltaTime);

		ApplyGravity ();
	}

	void ApplyGravity() {
		if (state == State.Falling) {
			velocity += new Vector2(0, -1)*gravity;
		}
	}

	void ProcessInput(PlayerInputNew.PlayerInput input) {
		movementDir = input.movementDirection;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		Debug.Log ("COLLIDED");
	}
}
