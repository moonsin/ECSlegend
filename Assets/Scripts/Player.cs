using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;


public class Player : MovingObjects {

	public GameObject[] moveRangeTiles;
	public GameObject[] attackRangeTiles;
	public int occupation;//1-knight,2-spearman,3-achrer,4-mage

	private Transform MoveRange;
	private Transform AttackRange;
	private Transform playerHolder;
	private Vector3 clickPos;
	public int attackPower = 1;

	private bool alreadyMoving = false;
	private Vector3 newPos;
	private int movingToNum;

	public static Player controllingPlayer = null;
	private static bool moving = false;
	private static bool attacking = false;

	public class AdjustPosition{
		public float x;
		public float y;


		public AdjustPosition(Vector3 position){
			x = position.x - 0.5f;
			y = position.y - 0.5f;
		}
	}

	// Use this for initialization
	void Start () {
		MoveRange = new GameObject ("MoveRange").transform;
		AttackRange = new GameObject ("AttackRange").transform;
		GameManager.instance.AddPlayerToList (this);
		occupation = 4;
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
	void setNewInstance(Vector3 pos,Transform range, GameObject[] tiles){
		GameObject toInstantiate = tiles [Random.Range (0, tiles.Length)];

		toInstantiate.transform.localScale = new Vector3 (1f, 1f, 0f);
		GameObject instance = Instantiate (toInstantiate, pos, Quaternion.identity) as GameObject;
		instance.transform.SetParent (range);
	}

	public void showRange(){

		playerHolder = this.transform;
		setMoveRange (playerHolder.position.x, playerHolder.position.y);

		foreach (Vector3 element in moveRanges) {
			setNewInstance (element, MoveRange, moveRangeTiles);
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

	void showAttackRange(){
		if(occupation == 1){
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					if (i != 0 || j != 0) {
						setNewInstance (new Vector3 (transform.position.x + i, transform.position.y + j, -0.5f), AttackRange, attackRangeTiles);
					}
				}
			}
		}
		else if(occupation == 2){
			for (int i = -2; i <= 2; i++) {
				for (int j = -2; j <= 2; j++) {
					if ((i == 0 || j == 0) && !(i == 0 && j == 0)) {
						print (new Vector3 (transform.position.x + i, transform.position.y + j, -0.5f));
						setNewInstance (new Vector3 (transform.position.x + i, transform.position.y + j, -0.5f), AttackRange, attackRangeTiles);

					}
				}
			}
		}
		else if(occupation == 3){
			for (int i = -2; i <= 2; i++) {
				for (int j = -2; j <= 2; j++) {
					if (Mathf.Abs (i) > 1 || Mathf.Abs (j) > 1 && (i != 0 || j != 0)) {
						setNewInstance (new Vector3 (transform.position.x + i, transform.position.y + j, -0.5f), AttackRange, attackRangeTiles);
					}
				}
			}
		}
		else if(occupation == 4){
			for (int i = -2; i <= 2; i++) {
				for (int j = -2; j <= 2; j++) {
					if (Mathf.Abs (i) + Mathf.Abs (j) <= 2 && (i != 0 || j != 0)) {
						setNewInstance (new Vector3 (transform.position.x + i, transform.position.y + j, -0.5f), AttackRange, attackRangeTiles);
					}
				}
			}
		}else{
			for (int i = -attackPower; i <= attackPower; i++) {
				for (int j = -attackPower; j <= attackPower; j++) {
					if (Mathf.Abs (i) + Mathf.Abs (j) <= attackPower && (i != 0 || j != 0)) {
						setNewInstance (new Vector3 (transform.position.x + i, transform.position.y + j, -0.5f), AttackRange, attackRangeTiles);
					}
				}
			}
		} 
	}






	// Update is called once per frame
	void Update () {
		clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (moving == true && controllingPlayer != this) {
			return;
		} else if (moving == false || controllingPlayer == this ) {
			

			if (Input.GetMouseButtonUp (0) && IsMouseOnPlayer (clickPos)) {
				if (hasMoved && !hasAttacked) {
					if (!attacking) {
						showAttackRange ();
					} else {
					}
				} else {
					if (!moving) {
						showRange ();
					} else {
						deleteMoveRange ();
					}
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
			if (Vector3.Distance (this.transform.position, bestPath [i]) >= 0.01 && movingToNum == i) {
				move (alreadyMoving, bestPath [i]);
			} else if (Vector3.Distance (this.transform.position, bestPath [i]) <= 0.01 && movingToNum == i){
				movingToNum -= 1;
			}
		}

		if (Vector3.Distance (this.transform.position, newPos) <= 0.01) {
			alreadyMoving = false;
			hasMoved = true;
			//turnFinished = true;
			deleteMoveRange ();
		}

	}
}
