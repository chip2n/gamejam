using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerIndicator : MonoBehaviour {
	public PlayerController targetPlayer;
	public Sprite[] indicatorSprites;
	Image img;

	// Use this for initialization
	void Start () {
		img = GetComponentInChildren<Image> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (targetPlayer) {
			if (targetPlayer.playerNumber <= indicatorSprites.Length) {
				img.enabled = true;
				img.sprite = indicatorSprites [targetPlayer.playerNumber - 1];
				transform.position = new Vector3 (targetPlayer.transform.position.x, targetPlayer.transform.position.y + 1.3f, targetPlayer.transform.position.z);
			}
		} else {
			img.enabled = false;
		}
	}
}
