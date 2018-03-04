using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;


public class BoardManager : MonoBehaviour {

	[Serializable]
	public class Count{

		public int minimum;
		public int maximum;

		public Count (int min, int max){
			minimum = min;
			maximum = max;
		}
	}

	public int colums = 10;
	public int rows = 10;
	public GameObject[] floorTiles;
	public GameObject[] outerTreeTiles;
	public GameObject[] treeTiles;
	public Count treeCount = new Count (5, 9);

	private Transform boardHolder;
	private List<Vector3> gridPositions = new List<Vector3> ();




	void InitialiseList(){
		gridPositions.Clear ();

		for (int x = 0; x < colums ; x++) {
			for (int y = 0; y < rows ; y++) {
				gridPositions.Add (new Vector3 (x, y, 0f));
			}
		}
	}
		
	void BoardSetup(){
		boardHolder = new GameObject ("board").transform;

		for (int x = -2; x < colums + 2; x++) {
			for (int y = -2; y < rows + 2; y++) {
				GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];
				if (x <= -1 || x >= colums || y <= -1 || y >= rows) {
					toInstantiate = outerTreeTiles [Random.Range (0, outerTreeTiles.Length)];
				}

				toInstantiate.transform.localScale = new Vector3 (0.78f, 0.78f, 0f);
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;
				instance.transform.SetParent (boardHolder);
			}
		}
	}

	public void SetupScene(){
		BoardSetup ();
		InitialiseList ();

		LayoutObjectAtRandom (treeTiles, treeCount.minimum, treeCount.maximum);
	}

	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range (minimum, maximum);
		for (int i = 0; i < objectCount; i++) {
			Vector3 randomPosition = RandomPosition ();

			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];

			tileChoice.transform.localScale = new Vector3 (0.78f, 0.78f, 0f);
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	Vector3 RandomPosition(){
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);

		return randomPosition;
	}




	// Use this for initialization
	void Start () {
		
	}


	// Update is called once per frame
	void Update () {

	}
}
