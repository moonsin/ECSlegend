using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	private Vector3 newPos;

	public static Player controllingPlayer;
	public static string buttonChoice = null;
	private bool playerBusy = false;
	private bool moving = false;
	private bool attacking = false;
	private static bool OverOtherFinish = false;
	private TutorialManager tutorial;

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
		tutorial = GameManager.instance.GetComponent<TutorialManager> ();
			
		//occupation = 4;
		fullHP = hp;
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
		moving = false;
		//moveRanges.Clear ();

	}
	void deleteAttackRange(){
		attackRangeInstances = GameObject.FindGameObjectsWithTag ("attackRange");
		Thread.Sleep (50);
		foreach (GameObject element in attackRangeInstances) {
			Destroy (element);
		}
		attacking = false;
		//moveRanges.Clear ();

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

	private bool IsMouseOnAttackRange(Vector3 mousePos){
		print (mousePos); 
		mousePos.z = -0.5f;
		Collider2D h = Physics2D.OverlapPoint (mousePos);

		if (h == null) {
			return false;
		} else {
			print (h.name == "attackRange(Clone)");
			return(h.name == "attackRange(Clone)");
		}
	}

	private bool isInEnemy(Vector3 mousePos){
		mousePos.z = -0.5f;
		Collider2D h = Physics2D.OverlapPoint (mousePos);

		if (h != null) {
			if (h.tag == "Enemy") {
				return true;
			}
		}
		return false;
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



	void TemporarilyFinishi(){
		playerBusy = false;
		controllingPlayer = null;
		OverOtherFinish = false;
	}


	// Update is called once per frame
	void Update () {
		
		if (GameManager.instance.isAllPlayersdead()) {
			GameManager.instance.gameOver.enabled = true;
			return;
		}
		if (GameManager.instance.isAllEnemiesdead()) {
			GameManager.instance.Victory.enabled = true;
			GameManager.instance.victory = true;
			return;
		}

		if (GameManager.instance.doingSetup) {
			return;
		}

		if (isDead) {
			GameObject.Find (this.name + "HpIndicator").GetComponent<Text> ().text = "";
			Destroy (GameObject.Find (this.name + "HpIndicator"));
			GameManager.instance.players.Remove (this);
			Destroy (this.gameObject);
		} 
	
		/*
		if (hasMoved && hasAttacked) {
			turnFinished = true;
			return;
		}
		*/

		clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		if (playerBusy == false) {
			if (turnFinished == true) {
				return;
			}
			if (Input.GetMouseButtonUp (0) && IsMouseOnPlayer (clickPos) && !OverOtherFinish) {
				controllingPlayer = this;
				playerBusy = true;
				OverOtherFinish = true;
				GameManager.instance.ShowPlayerMenu (this);

			} else {
				return;
			}
		} else if (playerBusy == true && controllingPlayer == this && ! alreadyMoving) {
			if (Input.GetMouseButtonUp (0) && IsMouseOnPlayer (clickPos)) {
				if (moving) {
					deleteMoveRange ();
					GameManager.instance.ShowPlayerMenu (this);
				}
				if (attacking) {
					deleteAttackRange ();
					GameManager.instance.ShowPlayerMenu (this);
				} else {
					GameManager.instance.HidePlayerMenu ();
					TemporarilyFinishi ();
				}
			} else if (Input.GetMouseButtonUp (0) && IsMouseOnMoveRange (clickPos)) {
				newPos = dirPos (clickPos);
				alreadyMoving = true;
				moveFinished = false;

				//setMoveRange (this.transform.position.x, this.transform.position.y);
				setBestPath (newPos);
				movingToNum = bestPath.Count - 1;
				moveRanges.Clear ();

			} else if (Input.GetMouseButtonUp (0) && IsMouseOnAttackRange (clickPos)){
				deleteAttackRange ();
				if (isInEnemy (clickPos)) {
					Collider2D h = Physics2D.OverlapPoint (clickPos);
					Enemy target = new Enemy ();
					for (int i = 0; i < GameManager.instance.enemies.Count; i++) {
						if (h.name == GameManager.instance.enemies [i].name) {
							target = GameManager.instance.enemies [i];
							break;
						}
					}
					attackObject (target);
					hasAttacked = true;
				}
				TemporarilyFinishi ();
			}else if (Input.GetMouseButtonUp (0)) {
				if (moving) {
					deleteMoveRange ();
					TemporarilyFinishi ();
				}
				else if (attacking) {
					deleteAttackRange ();
					TemporarilyFinishi ();
				} else {
					GameManager.instance.HidePlayerMenu ();
					if (buttonChoice != null) {
						if (buttonChoice == "move" && hasMoved == false) {
							showRange ();
							tutorial.firstClickMove = true;
							buttonChoice = null;
						} else if (buttonChoice == "attack" && hasAttacked == false) {
							if (this.name == "archer") {
								tutorial.firstClickArcherAttack = true;
							} else if (this.name == "mage") {
								tutorial.firstClickMageAttack = true;
							} else if (this.name == "knight") {
								tutorial.firstClickKnightAttack = true;
							} else if (this.name == "Berserker") {
								tutorial.firstClickBerserkerAttack = true;
							}

							showAttackRange ();
							buttonChoice = null;
							attacking = true;
						} else if (buttonChoice == "rest") {
							tutorial.firstRest = true;
							turnFinished = true;
							buttonChoice = null;
							TemporarilyFinishi ();
						} 
					} else {
						TemporarilyFinishi ();
					}
				}

			}
		} 



	if(newPos!=new Vector3(-1000f,-1000f,-1000f) ){
		for (int i = bestPath.Count-1; i >=0 ; i--) {
			if (Vector3.Distance (this.transform.position, bestPath [i]) >= float.Epsilon && movingToNum == i) {
				move (alreadyMoving, bestPath [i]);
			} else if (Vector3.Distance (this.transform.position, bestPath [i]) <= float.Epsilon && movingToNum == i){
				movingToNum -= 1;
			}
		}

		if (Vector3.Distance (this.transform.position, newPos) <= float.Epsilon && !moveFinished) {

			newPos = new Vector3(-1000f,-1000f,-1000f);
			moveFinished = true;
			alreadyMoving = false;
			hasMoved = true;
			deleteMoveRange ();
			TemporarilyFinishi ();
			//animator.SetTrigger (this.name + "WalkDone");
		}
	}

}
}
