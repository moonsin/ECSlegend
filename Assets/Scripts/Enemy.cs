using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObjects {
	Vector3 target;
	Vector3 BestPlace;
	// Use this for initialization
	void Start () {
		hp = 10;
		attack = 30;
		defense = 1;
		MovePoints = 3;
		GameManager.instance.AddEnemyToList (this);
	}

	private List<Vector3> getNerestList(Vector3 Pos){

		List<Vector3> list;
		list = new List<Vector3> ();
		Vector3 n = new Vector3 (Pos.x, Pos.y + 1f, 0f);
		Vector3 s = new Vector3 (Pos.x, Pos.y - 1f, 0f);
		Vector3 w = new Vector3 (Pos.x + 1f, Pos.y, 0f);
		Vector3 e = new Vector3 (Pos.x - 1f, Pos.y, 0f);


		list.Add(n);
		list.Add(s);
		list.Add(w);
		list.Add(e);

		return list;
	}

	private bool isNearPlayer(Vector3 Pos){
		List<Vector3> list = getNerestList(Pos);
		for(int i = 0 ; i< list.Count; i++){
			Collider2D h = Physics2D.OverlapPoint (list[i]);
			if (h != null) {
				if (h.tag == "Player") {
					return true;
				}
			}
		}
		return false;
	}

	private Player getNearestPlayer(Vector3 Pos){
		List<Vector3> list = getNerestList(Pos);
		Player playerTarget = new Player ();

		for(int i = 0 ; i< list.Count; i++){
			Collider2D h = Physics2D.OverlapPoint (list[i]);
			if (h != null) {
				if (h.tag == "Player") {
					for (int i2 = 0; i2 < GameManager.instance.players.Count; i2++) {
						if (h.name == GameManager.instance.players [i2].name) {
							playerTarget = GameManager.instance.players [i2];
							break;
						}
					}
				}
			}
		}
		return playerTarget;
	}

	public void MoveEnemy(){
		if (isNearPlayer (transform.position)) {
			return;
		} else {
			setMoveRange (transform.position.x, transform.position.y);
			target = FindTarget ();

			BestPlace = new Vector3 (-1f, -1f, -1f);
			float shortestDis = 10000;
			for (int i = 0; i < moveRanges.Count; i++) {
				float distance = Mathf.Abs (Vector3.Distance (target, moveRanges [i]));
				if (distance < shortestDis) {
					shortestDis = distance;
					BestPlace = moveRanges [i];
				}
			}
			print (shortestDis);
			alreadyMoving = true;
			moveFinished = false;
			setBestPath (BestPlace);
			movingToNum = bestPath.Count - 1;
			moveRanges.Clear ();
		}

	}

	Vector3 FindTarget(){
		
		List<Player> players = GameManager.instance.GetAllPlayers();

		float shortestDis = 10000;
		Vector3 target = new Vector3(0f,0f,0.5f);

		for (int i = 0; i < players.Count; i++) {
			Vector3 playerPos = players [i].transform.position;
			float distance = Mathf.Abs(Vector3.Distance (this.transform.position, playerPos));
			if (distance < shortestDis) {
				shortestDis = distance;
				target = playerPos;
			}
		}

		return target;
	}

	public bool GetIsDead(){
		return isDead;
	}


	Player FindPlayerOnVector3(Vector3 v3, Player player){
		Player[] players = GameObject.FindObjectsOfType<Player> ();
		for (int i = 0; i < players.Length; i++) {
			if (players [i].transform.position == v3) {
				return players [i];
			}
		}
		return player;
	}

	public void EnemyAttack(){
		if (isNearPlayer(transform.position)) {
			Player nerestPlayer = getNearestPlayer (transform.position);
			attackPlayer (nerestPlayer);
		} 
		TurnFinish ();
	}

	public void TurnFinish(){
		turnFinished = true;
	}



	// Update is called once per frame
	void Update () {
		if ( BestPlace != new Vector3(-1000f,-1000f,-1000f)){
			for (int i = bestPath.Count-1; i >=0 ; i--) {
				if (Vector3.Distance (this.transform.position, bestPath [i]) >= 0.01 && movingToNum == i) {
					move (alreadyMoving, bestPath [i]);
				} else if (Vector3.Distance (this.transform.position, bestPath [i]) <= 0.01 && movingToNum == i){
					movingToNum -= 1;
				}
			}
				
			if (Vector3.Distance (this.transform.position, BestPlace) <= 0.001 && !moveFinished) {
				BestPlace = new Vector3 (-1000f, -1000f, -1000f);
				moveFinished = true;
				alreadyMoving = false;
				hasMoved = true;
			}
		}

		if (isDead) {
			Destroy (this.gameObject);
		}
			
	
	}
}
