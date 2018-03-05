using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MovingObjects : MonoBehaviour {

	public float moveTime = 0.1f;
	public GameObject[] moveRangeInstances;
	public GameObject[] attackRangeInstances;
	public int speed;
	public GameObject HPindicator;

	public int MovePoints;
	public int hp;
	public int attack;
	public int defense;
	public int fullHP;

	public bool hasMoved = false;
	public bool hasAttacked = false;

	public bool turnFinished = false;
	protected bool isDead = false;
	protected List<Vector3> moveRanges = new List<Vector3> ();
	protected List<Vector3> bestPath = new List<Vector3> ();
	protected int[] bestPathArray;
	protected GameObject movingObj;
	protected string movingPlayer;
	protected int[,] PathArray;
	protected int[] dis;
	protected int movingToNum;
	protected bool moveFinished = false;
	protected bool alreadyMoving = false;

	private GameObject objHpindicator;

	// Use this for initialization
	protected void inItHpIndicator (Transform range ){
		//HPindicator.GetComponent<Text> ().text = "HP" + Convert.ToString(hp)+"/"+Convert.ToString(fullHP);

		HPindicator.GetComponent<Text> ().text = Convert.ToString (this.name);
		objHpindicator = HPindicator;


		Vector3 pos = transform.position;
		print (this.name);
		print (pos);
		Vector3 newPos = Camera.main.WorldToScreenPoint(new Vector3 (pos.x, pos.y+0.6f, -0.5f));
		print (newPos);
		objHpindicator.transform.position = newPos;

		GameObject instance = Instantiate (objHpindicator, newPos, Quaternion.identity) as GameObject;
		HPindicator.GetComponent<Text> ().text = "";

		instance.transform.SetParent (range);
	}

	protected void Start () {
		Transform range = GameObject.Find ("HpIndicator").transform;
		//inItHpIndicator (range);
	}

	public void loseHp(int hurt){
		int originHp = hp;
		hp -= hurt;
	}

	public int GetDefend(){
		return defense;
	}

	public int GetHp(){
		return hp;
	}

	private bool IsHinder(Vector3 pos){
		Collider2D h = Physics2D.OverlapPoint (pos);
		return !(h == null);
	}

	private void setPathArrayByPos(Vector3 pos, int i){
		int index = moveRanges.IndexOf(pos);
		if(index != -1){
			PathArray[i,index] = 1;
		}
	}

	private void initPathArray(int Count){
		PathArray = new int[Count,Count];
		for(int i = 0 ; i < Count;i++){
			for (int i2 = 0; i2 < Count; i2++) {
				PathArray[i, i2] = 100000;
			}
		}
	}

	private void initBestPathArray(int Count){
		bestPathArray = new int[moveRanges.Count];

		for (int i = 0; i < moveRanges.Count; i++) {
			bestPathArray [i] = 0;
		}
	}
		
	public void setMoveRange(float xpos, float ypos){

		Vector3 newPos1,newPos2;
		moveRanges.Clear ();
		moveRanges.Add(new Vector3 (xpos, ypos, -0.5f));
			
		for (int i = 0; i <= MovePoints; i++) {

			if (i == 0) {
				for (int i2 = -MovePoints; i2 <= MovePoints; i2++) {
					newPos1 = new Vector3 (xpos + i2, ypos + i, -0.5f);
					if (!IsHinder(newPos1)){
						moveRanges.Add (newPos1);
					}
				}
			} else {
				for (int i2 = -MovePoints+i; i2 <= MovePoints-i; i2++) {
					newPos1 = new Vector3 (xpos + i2, ypos + i, -0.5f);
					newPos2 = new Vector3 (xpos + i2, ypos - i, -0.5f);
					if (!IsHinder(newPos1)) {
						moveRanges.Add (newPos1);
					}
					if (!IsHinder(newPos2)) {
						moveRanges.Add (newPos2);
					}
				}
			}
		}


		initPathArray(moveRanges.Count);
		initBestPathArray (moveRanges.Count);

		for (int i = 0; i < moveRanges.Count; i++) {
			PathArray [i,i] = 0;
			Vector3 currentPos = moveRanges [i];
			Vector3 leftPos = new Vector3 (currentPos.x - 1, currentPos.y, currentPos.z);
			Vector3 rightPos = new Vector3 (currentPos.x + 1, currentPos.y, currentPos.z);
			Vector3 topPos = new Vector3 (currentPos.x, currentPos.y + 1, currentPos.z);
			Vector3 downPos = new Vector3 (currentPos.x, currentPos.y - 1, currentPos.z);
			setPathArrayByPos (leftPos, i);
			setPathArrayByPos (rightPos, i);
			setPathArrayByPos (topPos, i);
			setPathArrayByPos (downPos, i);
		}

		dis = new int[moveRanges.Count];
		for (int i = 0; i < moveRanges.Count; i++) {
			dis [i] = PathArray [0, i];
		}
			
		bool[] doneArray = new bool[moveRanges.Count];
		doneArray [0] = true;
		for (int i = 1; i < moveRanges.Count; i++) {
			doneArray [i] = false;
		}

		for(int i = 0 ; i< moveRanges.Count-1;i++){
			int bestIndex = 0;
			int nerestPathLength = 100000;

			for (int i2 = 0; i2 < moveRanges.Count; i2++) {
				if (dis [i2] <= nerestPathLength && !doneArray [i2]) {
					bestIndex = i2;
					nerestPathLength = dis [i2];
				}
			}
		
			doneArray [bestIndex] = true;
			for (int i2 = 0; i2 < moveRanges.Count; i2++) {
				int newPathLength = PathArray [bestIndex, i2] + nerestPathLength;
				if (newPathLength < dis [i2]) {
					bestPathArray [i2] = bestIndex;
					dis [i2] = newPathLength;
				}
			}
		}

		for (int i = 0; i < moveRanges.Count; i++) {
			if (dis [i] > MovePoints) {
				moveRanges[i] = new Vector3(xpos,ypos,-0.5f);
			}
		}
		//moveRanges.Remove(moveRanges[0]);

		int originIndex = moveRanges.IndexOf (new Vector3 (xpos, ypos, -0.5f));

		while ( originIndex != -1) {
			moveRanges[originIndex] = new Vector3 (1000+ xpos, 1000+ ypos, -0.5f);
			originIndex = moveRanges.IndexOf (new Vector3 (xpos, ypos, -0.5f));
		}

	}

	protected void setBestPath(Vector3 newPos){
		
		bestPath = new List<Vector3> ();
		int targetIndex = moveRanges.IndexOf (newPos);
		bestPath.Add (newPos);
		print (targetIndex);
		while(bestPathArray[targetIndex]!=0){
			bestPath.Add (moveRanges [bestPathArray [targetIndex]]);
			targetIndex = bestPathArray [targetIndex];
		}
	}

	protected void move(bool alreadyMoving, Vector3 newPos){
		if (alreadyMoving) {
			
			float step = speed * Time.deltaTime;
			this.transform.position = Vector3.MoveTowards (this.transform.position, newPos, step);

		}
	}

	public bool GetTurnFinish(){
		return turnFinished;
	}

	protected void attackObject(Enemy obj){
		int hurt = attack - obj.defense > 0 ? attack - obj.defense : 0;
		obj.hp -= hurt;
		if (obj.hp <= 0) {
			obj.isDead = true;
		}
	}

	protected void attackPlayer(Player obj){
		int hurt = attack - obj.defense > 0 ? attack - obj.defense : 0;
		obj.hp -= hurt;
		if (obj.hp <= 0) {
			obj.isDead = true;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
