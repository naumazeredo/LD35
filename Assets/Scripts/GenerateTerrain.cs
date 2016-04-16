using UnityEngine;
using System.Collections;

public class GenerateTerrain : MonoBehaviour {

	public GameObject ground;
	public GameObject water;
	const float groundSizeY = 4.00f;
	const float groundY = -4.00f;
	float diffGroundX = 0.0f;
	float lastGroundX = 0.0f;
	float lastScale = 0;
	const int maxNumBlocks = 5;
	enum FloorType { Floor, Water, Spider };

	public void GenerateBlock(){
		var scale = Random.Range(15.0f, 25.0f);
		lastGroundX += diffGroundX + scale / 2 + lastScale / 2;
		var groundBlock = Instantiate (ground, new Vector3 (lastGroundX, groundY), Quaternion.identity) as GameObject;
		groundBlock.transform.localScale = new Vector3 (scale, groundSizeY);
		lastScale = scale;
		diffGroundX = Random.Range(5.0f, 10.0f);
	}

	public void GenerateWater(){
		var scale = Random.Range(15.0f, 25.0f);
		lastGroundX += scale / 2 + lastScale / 2;
		var groundBlock = Instantiate (water, new Vector3 (lastGroundX, groundY), Quaternion.identity) as GameObject;
		groundBlock.transform.localScale = new Vector3 (scale+25f, groundSizeY);
		lastScale = scale;
		diffGroundX = Random.Range(5.0f, 10.0f);
	}

	FloorType GetFloorType(){
		if (Random.Range (1, 5) == 2) {
			return FloorType.Water;
		}
		return FloorType.Floor;
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < maxNumBlocks; i++) {
			if (GetFloorType() == FloorType.Water) {
				GenerateWater ();
			} else {
				GenerateBlock();
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		var vertExtent = Camera.main.orthographicSize;
		var hortExtent = vertExtent * Screen.width / Screen.height;
		if (Camera.main.transform.position.x +hortExtent >lastGroundX) {
			for (int i = 0; i < maxNumBlocks; i++) {
				if (GetFloorType() == FloorType.Water) {
					GenerateWater ();
				} else {
					GenerateBlock();
				}
			}
		}
	}
}
