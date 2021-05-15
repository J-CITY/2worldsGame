using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//types:
//normal, finish, start, bomb, river(smth else), portal, button,
//door, bridge, lift vertical, lift horizontal
//objects:
//star, box, goal, enemy
//enviroment

//player:
// energy for swap,
// energy for steps,
// shield
// step back,
// attack

/*
 * "id": 0,
 * "swaps": 10,
 * "steps": -1,
 * "style": "default",
 * "size": [10, 10],
 * "layerSize": 2,
 * "layer0": [
 *		{ 
 *		  "x":0,
 *		  "y":0,
 *		  "height": 2,
 *			"type": "normal",
 *			"object": "star",
 *			"style": "normal",
 *			//"enemyPath": [] //if object enemy
 *		},...
 * ]
 */

public class BlockItem {
	public float size = 1;
	public BlockController.Type type = BlockController.Type.NORMAL;

	public BlockItem() { }
	public BlockItem(float sz, BlockController.Type t) {
		size = sz;
		type = t;
	}
}

public class MapController : MonoBehaviour {
	public GameObject blockPref;
	public Material blockMaterial;
	public Material blockDieMaterial;
	public Material blockFinishMaterial;

	public Material skyBox1;
	public Material skyBox2;

	public PlayerController player;
	public GameObject camera;

	public int curLayer = 0;
	public List<BlockItem[][]> layers;
	public GameObject[][] blocks = new GameObject[10][];
	public float duration = 1.0F;

	public float fadeSpeed = 1.0f;

	Color colorStart = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 255.0f / 255.0f);
	Color colorEnd = new Color(0 / 255.0f, 11.0f / 255.0f, 40.0f / 255.0f, 255.0f / 255.0f);
	private float playerSize = 1.0f;
	float lastPress = 0.0f;

	enum sbType {
		NONE,
		SHOW,
		HIDE
	}
	sbType sbt = sbType.NONE;
	float curLerp = 0;

	void Start() {

		BlockItem[][] layer0 = {
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(4, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.FINISH),}
		};

		BlockItem[][] layer1 = {
			new BlockItem[] { new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(8, BlockController.Type.NORMAL),new BlockItem(5, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(9, BlockController.Type.NORMAL),new BlockItem(6, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(8, BlockController.Type.NORMAL),new BlockItem(7, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(7, BlockController.Type.NORMAL),new BlockItem(8, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(6, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.DIE),   new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(6, BlockController.Type.NORMAL),new BlockItem(9, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(5, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(4, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(4, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(3, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),},
			new BlockItem[] { new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(1, BlockController.Type.NORMAL),new BlockItem(2, BlockController.Type.NORMAL),}
		};
		layers = new List<BlockItem[][]>();
		layers.Add(layer0);
		layers.Add(layer1);
		initMap();
	}

	void initMap() {
		for (var i = 0; i < layers[curLayer].Length; i++) {
			blocks[i] = new GameObject[10];
			for (var j = 0; j < layers[curLayer][0].Length; j++) {
				var block = Instantiate(blockPref, new Vector3(i, 0, j), Quaternion.identity);
				block.GetComponent<BlockController>().yScale = layers[curLayer][i][j].size;
				block.GetComponent<MeshRenderer>().material = getMaterial(layers[curLayer][i][j].type);
				blocks[i][j] = block;
			}
		}
	}
	void updateMap() {
		//RenderSettings.skybox = curLayer == 0 ? skyBox1 : skyBox2;
		sbt = sbType.HIDE;
		curLerp = 1.0f - curLerp;
		if (curLerp < 0) curLerp = 0;
		if (curLerp > 1) curLerp = 1;
		for (var i = 0; i < layers[curLayer].Length; i++) {
			for (var j = 0; j < layers[curLayer][0].Length; j++) {
				if (i == (int)player.x && j == (int)player.y) {
					continue;
				}
				var block = blocks[i][j];
				block.GetComponent<BlockController>().yScale = layers[curLayer][i][j].size;
				block.GetComponent<MeshRenderer>().material = getMaterial(layers[curLayer][i][j].type);
			}
		}
	}

	Material getMaterial(BlockController.Type t) {
		switch (t) {
			case BlockController.Type.NORMAL:
				return blockMaterial;
			case BlockController.Type.DIE:
				return blockDieMaterial;
			case BlockController.Type.FINISH:
				return blockFinishMaterial;
			default:
				return blockMaterial;
		}
	}

	void swapMap() {
		lastPress = Time.time;
		if (layers[(curLayer + 1) % 2][player.x][player.y].size == playerSize) {
			curLayer = (curLayer + 1) % 2;
			updateMap();
		}
		else {
			curLayer = (curLayer + 1) % 2;
			updateMap();
			StartCoroutine(WrongSwap());
		}
	}

	public void onClickSwapMap() {
		swapMap();
	}
	public void onClickRestartMap() {
		isRestartByPress = true;
		StartCoroutine(RestartLevel());
	}

	void Update() {
		if (Time.time - lastPress > 0.1f && Input.GetKeyUp(KeyCode.Space)) {
			swapMap();
		}
		CheckPlayerStatus();

		UpdateSkybox();
	}

	void UpdateSkybox() {
		if (sbt == sbType.NONE) {
			return;
		}
		curLerp += fadeSpeed* Time.deltaTime;
		if (curLerp >= 1.0f) {
			if (sbt == sbType.HIDE) {
				sbt = sbType.SHOW;
				RenderSettings.skybox = curLayer == 0 ? skyBox1 : skyBox2;
				RenderSettings.skybox.SetColor("_TintColor", colorEnd);
			}
			else if (sbt == sbType.SHOW) {
				sbt = sbType.NONE;
			}
			curLerp = 0.0f;
			return;
		}
		if (curLayer == 0) {
			if (sbt == sbType.HIDE) {
				RenderSettings.skybox.SetColor("_TintColor", Color.Lerp(colorStart, colorEnd, curLerp));
			}
			else {
				RenderSettings.skybox.SetColor("_TintColor", Color.Lerp(colorEnd, colorStart, curLerp));
			}
		}
		else {
			if (sbt == sbType.HIDE) {
				RenderSettings.skybox.SetColor("_TintColor", Color.Lerp(colorStart, colorEnd, curLerp));
			}
			else {
				RenderSettings.skybox.SetColor("_TintColor", Color.Lerp(colorEnd, colorStart, curLerp));
			}
		}
	}

	public bool isWin = false;
	public bool isRestartByPress = false;

	void CheckPlayerStatus() {
		if (layers[curLayer][player.x][player.y].type == BlockController.Type.DIE) {
			print("DIE!");
			StartCoroutine(RestartLevel());
			return;
		}
		if (layers[curLayer][player.x][player.y].type == BlockController.Type.FINISH) {
			print("WIN! --> NEXT LEVEL");
			isWin = true;
			StartCoroutine(RestartLevel());
			return;
		}
	}

	IEnumerator WrongSwap() {
		camera.GetComponent<StressReceiver>().InduceStress(0.2f);
		yield return new WaitForSeconds(0.2f);
		curLayer = (curLayer + 1) % 2;
		updateMap();
	}

	IEnumerator RestartLevel() {
		if (!isWin && !isRestartByPress) {
			camera.GetComponent<StressReceiver>().InduceStress(0.25f);
		}
		isWin = false;
		isRestartByPress = false;
		yield return new WaitForSeconds(0.3f);
		player.restart();
		curLayer = 0;
		updateMap();
	}
}
