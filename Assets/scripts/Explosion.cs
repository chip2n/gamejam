using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Explosion : MonoBehaviour {
	public GameObject hitboxPrefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AnimationDone() {
		Debug.Log ("IM HERE!");
		Destroy(gameObject);
	}
}
