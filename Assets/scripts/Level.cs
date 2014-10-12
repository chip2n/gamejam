using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System;
using System.Linq;

[Serializable]
public class Level : MonoBehaviour {
	public List<int> tiles;
	public List<List<int>> tileLayers;
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
		tileLayers = new List<List<int>> ();
		string json_string = File.ReadAllText(levelSource);
		JSONNode node = JSON.Parse (json_string);
		JSONArray layers = node["layers"].AsArray;

		for(int i = 0; i < layers.Count; i++) {
			JSONNode layer = layers[i];
			JSONArray data = layer["data"].AsArray;
			width = layer["width"].AsInt;
			height = layer["height"].AsInt;
			
			List<int> tiles = new List<int> ();
			for(int j = 0; j < data.Count; j++) {
				tiles.Add(data[j].AsInt);
			}

			tileLayers.Add(tiles);
		}


		CreateSprites();
	}

	void CreateSprites() {
		float layerZ = 0.0f;
		foreach(List<int> tiles in tileLayers) {
			for(int i = 0; i < tiles.Count; i++) {
				int tileIndex = tiles[i] - 1;
				if(tileIndex > -1) {
					int x = i % 32;
					int y = (height - 1) - i / 32;
					GameObject test = Instantiate (tilePrefab) as GameObject;
					test.transform.parent = this.transform;
					Tile tile = test.GetComponent<Tile>();
					tile.setPosition(x, y, layerZ);
					if(tileIndex == 3) {
						tile.collider2D.enabled = false;
					} else {
						test.layer = 9;
					}
					SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
					sr.sprite = GetSpriteByTileId(tiles[i]);

				}
			}
			layerZ -= 1.0f;
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
