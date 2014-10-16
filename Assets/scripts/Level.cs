using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using System.IO;
using System;
using System.Linq;

public struct TileData {
	public int id;
	public bool collidable;
	public string cls;
}

[Serializable]
public class Level : MonoBehaviour {
	public List<int> tiles;
	public List<TileData> tilesWithCol;
	public List<List<TileData>> tileLayers;
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
		tileLayers = new List<List<TileData>> ();
		tilesWithCol = new List<TileData> ();
		string json_string = File.ReadAllText(levelSource);
		JSONNode node = JSON.Parse (json_string);
		JSONArray layers = node["layers"].AsArray;
		JSONArray tilesets = node ["tilesets"].AsArray;
		JSONNode tileproperties = tilesets [0] ["tileproperties"];

		for(int i = 0; i < layers.Count; i++) {
			JSONNode layer = layers[i];
			JSONArray data = layer["data"].AsArray;
			JSONNode collidableNode = layer["properties"]["collidable"];
			bool collidable = collidableNode.AsInt == 1;
			width = layer["width"].AsInt;
			height = layer["height"].AsInt;
			
			List<TileData> tiles = new List<TileData> ();
			for(int j = 0; j < data.Count; j++) {
				TileData d = new TileData();
				d.id = data[j].AsInt - 1;
				d.collidable = collidable;
				//Debug.Log (tileproperties[data[j].AsInt]["class"].ToString());
				if (tileproperties[d.id.ToString()] != null) {
					d.cls = tileproperties[d.id.ToString ()]["class"];
				}
				tiles.Add(d);
			}

			tileLayers.Add(tiles);
		}


		CreateSprites();
	}

	void CreateSprites() {
		float layerZ = 0.0f;
		foreach(List<TileData> tiles in tileLayers) {
			for(int i = 0; i < tiles.Count; i++) {
				TileData d = tiles[i];
				int tileIndex = d.id;
				bool collidable = d.collidable;
				//int tileIndex = tiles[i] - 1;
				if(tileIndex > -1) {
					int x = i % 32;
					int y = (height - 1) - i / 32;
					GameObject test = Instantiate (tilePrefab) as GameObject;
					test.transform.parent = this.transform;
					Tile tile;
					if(d.cls == "lava") {
						tile = test.AddComponent<LavaTile>();
					} else {
						tile = test.AddComponent<Tile>();
					}

					tile.SetPosition(x, y, layerZ);
					tile.SetCollidable(collidable);
					SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
					sr.sprite = GetSpriteByTileId(tiles[i].id);

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
		Sprite[] sprites = Resources.LoadAll<Sprite> (@"sprites/level_tiles");
		foreach (Sprite s in sprites) {
			tileSprites.Add(s);
		}
		tileSprites = tileSprites.OrderBy (o => Convert.ToInt32(o.name)).ToList();
	}
}
