using UnityEngine;
using System.Collections;

public class TileProperty {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static TileProperty CreateFromString(string cls) {

		if (cls == "lava") {
			Debug.Log (cls);
			return new TileProperty ();
		} else if (cls == "") {
			return new TileProperty ();		
		} else {
			return null;		
		}
	}
}
