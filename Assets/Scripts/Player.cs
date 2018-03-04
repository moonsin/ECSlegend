using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class Player : MovingObjects {

	public int movePoints = 5;

	public GameObject[] moveRangeTiles;

	private Transform MoveRange;
	private Transform playerHolder;
	private Vector3 clickPos;

	private bool alreadyMoving = false;
	private Vector3 newPos;
	private int movingToNum;

	public static Player controllingPlayer = null;
	private static bool moving = false;

	// Use this for initialization
	void Start () {
		MoveRange = new GameObject ("MoveRange").transform;
		GameManager.instance.AddPlayerToList (this);
		base.Start ();
	}

	float getNearPost(float num){
		return (float)(Mathf.Floor (num)) + 0.5f;
	}

	Vector3 dirPos(Vector3 pos){
		return new Vector3 (getNearPost (pos.x), getNearPost (pos.y), -0.5f);
	}

	void deleteMoveRange(){
		moveRangeInstances = GameObject.FindGameObjectsWithTag ("MoveRanges");
		Thread.Sleep (50);
		foreach (GameObject element in moveRangeInstances) {
			Destroy (element);
		}
		//moveRanges.Clear ();
		moving = false;
		controllingPlayer = null;
	}

	public void showRange(){

		playerHolder = this.transform;
		setMoveRange (playerHolder.position.x, playerHolder.position.y, movePoints);

		foreach (Vector3 element in moveRanges) {
			print (element);
			GameObject toInstantiate = moveRangeTiles [Random.Range (0, moveRangeTiles.Length)];

			toInstantiate.transform.localScale = new Vector3 (1f, 1f, 0f);
			GameObject instance = Instantiate (toInstantiate, element, Quaternion.identity) as GameObject;
			instance.transform.SetParent (MoveRange);
		}

		controllingPlayer = this;
		moving = true;

	}

	private bool IsMouseOnPlayer(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else
			return (this.name == h.name);
	}

	private bool IsMouseOnMoveRange(Vector3 mousePos){
		Collider2D h = Physics2D.OverlapPoint (mousePos);
		if (h == null) {
			return false;
		}
		else 
			return ( h.name == "test(Clone)");
	}



	public bool GetTurnFinish(){
		return turnFinished;
	}

	// Update is called once per frame
	void Update () {
		clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (moving == true && controllingPlayer != this) {
			return;
		} else if (moving == false || controllingPlayer == this ) {

			if (hasMoved) {
				return;
			}
			if (Input.GetMouseButtonUp (0) && IsMouseOnPlayer (clickPos)) {
				if (!moving) {
					showRange ();
				} else {
					deleteMoveRange ();
				}
			} else if (Input.GetMouseButtonUp (0) && IsMouseOnMoveRange (clickPos)) {
				newPos = dirPos (clickPos);
				alreadyMoving = true;
				setBestPath (newPos);
				movingToNum = bestPath.Count - 1;


			} else if (Input.GetMouseButtonUp (0)) {
				deleteMoveRange ();
			}
		} 

		for (int i = bestPath.Count-1; i >=0 ; i--) {
			print (Vector3.Distance (this.transform.position, bestPath [i]));
			if (Vector3.Distance (this.transform.position, bestPath [i]) >= 0.01 && movingToNum == i) {
				move (alreadyMoving, bestPath [i]);
			} else if (Vector3.Distance (this.transform.position, bestPath [i]) <= 0.01 && movingToNum == i){
				movingToNum -= 1;
			}
		}

		if (Vector3.Distance (this.transform.position, newPos) <= 0.01) {
			alreadyMoving = false;
			hasMoved = true;
			turnFinished = true;
			deleteMoveRange ();
		}

	}
}
