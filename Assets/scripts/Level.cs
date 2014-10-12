using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System;
using System.Linq;

[Serializable]
public class Level : MonoBehaviour {
	public List<int> tiles;
	public string levelSource;
	public GameObject tilePrefab;
	public Texture2D tileMap;
	public List<Sprite> tileSprites;

	private int height;
	private int width;
	void Awake() {
		LoadSprites();
	}
	void Start() {
		string json_string = File.ReadAllText(levelSource);
		JSONNode node = JSON.Parse (json_string);
		JSONArray a = node["layers"].AsArray;
		
		JSONArray data = a[0]["data"].AsArray;
		width = a[0]["width"].AsInt;
		height = a[0]["width"].AsInt;

		tiles = new List<int> ();
		for(int i = 0; i < data.Count; i++) {
			tiles.Add(data[i].AsInt);
		}

		CreateSprites();
	}

	void CreateSprites() {
		for(int i = 0; i < tiles.Count; i++) {
			int tileIndex = tiles[i] - 1;
			if(tileIndex > -1) {
				int x = i % 32;
				int y = (height - 1) - i / 32;
				GameObject test = Instantiate (tilePrefab) as GameObject;
				test.transform.parent = this.transform;
				Tile tile = test.GetComponent<Tile>();
				tile.setPosition(x, y);
				if(tileIndex == 3) {
					tile.collider2D.enabled = false;
				} else {
					test.layer = 9;
				}
				SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
				sr.sprite = GetSpriteByTileId(tiles[i]);
			}
		}
	}

	Sprite GetSpriteByTileId(int id) {
		foreach (Sprite sp in tileSprites) {
			if(sp.name == id.ToString()) {
				return sp;
			}
		}

		return null;
	}


	void LoadSprites() {
		/*
		string spriteSheet = AssetDatabase.GetAssetPath(tileMap);
		Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath( spriteSheet )
			.OfType<Sprite>().ToArray();

		foreach (Sprite s in sprites) {
			tileSprites.Add(s);
		}
		*/

		Sprite[] sprites = Resources.LoadAll<Sprite> (@"sprites/tilemap");
		foreach (Sprite s in sprites) {
			tileSprites.Add(s);
		}
		tileSprites = tileSprites.OrderBy (o => o.name).ToList();

	}
}
