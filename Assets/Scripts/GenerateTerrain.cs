using UnityEngine;

public class GenerateTerrain : MonoBehaviour {

	public GameObject ground;
    public Sprite[] groundBorder;
    public Sprite[] groundInner;
	public GameObject water;
	public GameObject ceiling;
    public Sprite[] ceilingSprites;
	public GameObject breakable;
    public Sprite[] breakableSprites;
	public GameObject wall;
	const int ceilingY = 5;
	const int ceilingSizeY = 3;
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
				GameObject go = Instantiate (objectType , new Vector3 (posXBegin + i, posYBegin+j), Quaternion.identity) as GameObject;
                if (objectType == ceiling) {
                    go.GetComponent<SpriteRenderer>().sprite = ceilingSprites[Random.Range(0, ceilingSprites.Length)];
                    go.GetComponent<SpriteRenderer>().color = Color.white;
                }
                /*
                if (j == sizeY - 1) go.GetComponent<SpriteRenderer>().sprite = groundBorder[Random.Range(0, groundBorder.Length)];
                else go.GetComponent<SpriteRenderer>().sprite = groundInner[Random.Range(0, groundInner.Length)];
                */
            }
        }
	}

	public void AddBreakable(float posBegin, float randomY){
		for (int j = -1; j < 7; j++) {
			GameObject go = Instantiate (breakable , new Vector3 (posBegin+1, j+randomY), Quaternion.identity) as GameObject;
            go.GetComponent<SpriteRenderer>().sprite = breakableSprites[Random.Range(0, breakableSprites.Length)];
            go.GetComponent<SpriteRenderer>().color = Color.white;
		}
	}

	public void AddWall(float posBegin, float randomY){
		for (int i = 1; i < 4; i++) {
			for (int j = 0; j < 7; j++) {
				Instantiate (wall , new Vector3 (posBegin + i, j+randomY-0.4f), Quaternion.identity);
			}
		}
	}

	public void AddObstacle(float posXBegin, float sizeX, float randomY){
		for (int i = 2; i < sizeX - 5; i++) {
			var obstacle = Random.Range (1,15);
			if (obstacle == 1 || obstacle == 2) {
				AddWall (posXBegin + i, randomY);
				i += 5;
			} else if (obstacle == 2 || obstacle == 3) {
				AddBreakable (posXBegin+i, randomY);
				i += 3;
			}
		}
	}

	public void GenerateFloor(){
		var scale = Random.Range(15, 25);
		lastGroundX += diffGroundX + lastScale / 2;
		var randomY = Random.Range(-0.3f,0.3f);
		RenderTerrain (ground, scale, floorSizeY, lastGroundX, floorY+randomY);
		AddObstacle (lastGroundX, scale, randomY);
		lastGroundX+= + scale / 2;
		lastScale = scale;
		diffGroundX = Random.Range(5, 10);
	}

	public void GenerateWater(){
		var scale = Random.Range(15, 25);
		lastGroundX+=  lastScale / 2;
		RenderTerrain (water, scale+15, waterSizeY, lastGroundX-4, waterY+Random.Range(-0.3f,0.3f));
		lastGroundX+= + scale / 2;
		lastScale = scale;
		diffGroundX = Random.Range(5, 10);
	}

	public void GenerateCeiling(){
		var scale = Random.Range(15, 25);
		lastGroundX +=lastScale / 2;
		RenderTerrain (ceiling, scale+15, ceilingSizeY, lastGroundX-4, ceilingY);
		lastGroundX+= + scale / 2;
		lastScale = scale;
		diffGroundX = Random.Range(5, 10);
	}

	FloorType GetFloorType(){
		var floorType = Random.Range (1, 10);
		if(lastFloor != FloorType.Floor ){
			lastFloor = FloorType.Floor;
			return FloorType.Floor;
		}
		if (floorType <= 3) {
			lastFloor = FloorType.Water;
			return FloorType.Water;
		}
		else if(floorType <= 6){
			lastFloor = FloorType.Ceiling;
			return FloorType.Ceiling;
		}
		lastFloor = FloorType.Floor;
		return FloorType.Floor;
	}

	// Use this for initialization
	void Start () {
        /*
		for (int i = 0; i < maxNumBlocks; i++) {
			var terrainType = GetFloorType ();
			if (terrainType == FloorType.Water) {
				GenerateWater ();
			} else if (terrainType == FloorType.Ceiling) {
				GenerateCeiling ();
			}
			GenerateFloor();
		}
        */

        lastGroundX = 117f;
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
