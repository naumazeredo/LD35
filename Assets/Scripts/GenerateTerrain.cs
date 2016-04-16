using UnityEngine;
using System.Collections;

public class GenerateTerrain : MonoBehaviour {

	public GameObject ground;
	public GameObject water;
	public GameObject ceiling;
	const int cielingY = 5;
	const int cielingSizeY = 3;
	const int floorSizeY = 4;
	const int floorY = -5;
	const int waterSizeY = 4;
	const int waterY = -6;
	int diffGroundX = 0;
	float lastGroundX = 0.00f;
	int lastScale = 0;
	const int maxNumBlocks = 5;
	enum FloorType { Floor, Water, Ceiling };
	FloorType lastFloor = FloorType.Ceiling;

	public void RenderTerrain(GameObject objectType, int sizeX, int sizeY, float posXBegin, float posYBegin){
		for (int i = 0; i < sizeX; i++) {
			for (int j = 0; j < sizeY; j++) {
				Instantiate (objectType , new Vector3 (posXBegin + i, posYBegin+j), Quaternion.identity);
			}
		}
	}

	public void GenerateFloor(){
		var scale = Random.Range(15, 25);
		lastGroundX += diffGroundX + lastScale / 2;
		RenderTerrain (ground, scale, floorSizeY, lastGroundX, floorY+Random.Range(-0.5f,0.5f));
		lastGroundX+= + scale / 2;
		lastScale = scale;
		diffGroundX = Random.Range(5, 10);
	}

	public void GenerateWater(){
		var scale = Random.Range(15, 25);
		lastGroundX+=  lastScale / 2;
		RenderTerrain (water, scale+15, waterSizeY, lastGroundX-4, waterY+Random.Range(-0.5f,0.5f));
		lastGroundX+= + scale / 2;
		lastScale = scale;
		diffGroundX = Random.Range(5, 10);
	}

	public void GenerateCeiling(){
		var scale = Random.Range(15, 25);
		lastGroundX +=lastScale / 2;
		RenderTerrain (ceiling, scale+15, cielingSizeY, lastGroundX-4, cielingY);
		lastGroundX+= + scale / 2;
		lastScale = scale;
		diffGroundX = Random.Range(5, 10);
	}

	FloorType GetFloorType(){
		var floorType = Random.Range (1, 10);
		if(lastFloor !=FloorType.Floor ){
			lastFloor = FloorType.Floor;
			return FloorType.Floor;
		}
		if (floorType == 1 || floorType == 2) {
			lastFloor = FloorType.Water;
			return FloorType.Water;
		}
		else if(floorType == 3){
			lastFloor = FloorType.Ceiling;
			return FloorType.Ceiling;
		}
		lastFloor = FloorType.Floor;
		return FloorType.Floor;
	}

	// Use this for initialization
	void Start () {
		for (int i = 0; i < maxNumBlocks; i++) {
			var terrainType = GetFloorType ();
			if (terrainType == FloorType.Water) {
				GenerateWater ();
			} else if (terrainType == FloorType.Ceiling) {
				GenerateCeiling ();
			}
			GenerateFloor();
		}
	}
	
	// Update is called once per frame
	void Update () {
		var vertExtent = Camera.main.orthographicSize;
		var hortExtent = vertExtent * Screen.width / Screen.height;
		if (Camera.main.transform.position.x +hortExtent >lastGroundX) {
			for (int i = 0; i < maxNumBlocks; i++) {
				var terrainType = GetFloorType ();
				if (terrainType == FloorType.Water) {
					GenerateWater ();
				} else if (terrainType == FloorType.Ceiling) {
					GenerateCeiling ();
				}
				GenerateFloor();
			}
		}
	}
}
