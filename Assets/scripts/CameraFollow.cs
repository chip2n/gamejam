using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class CameraFollow : MonoBehaviour {
	public Transform followObject;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = new Vector3(followObject.position.x, followObject.position.y, transform.position.z);
	}
}
