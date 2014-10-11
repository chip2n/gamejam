using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Explosion : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void AnimationDone() {
		Destroy(gameObject);
	}
}
