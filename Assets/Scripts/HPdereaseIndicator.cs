using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class HPdereaseIndicator : MonoBehaviour {
	public Canvas canvas;
	public GameObject HpDecreseIndicator;
	public Player player;
	public Enemy enemy;
	private GameObject newHpDecreseIndicator;

	protected void inItHpDecreseIndicator(){
		Vector3 pos  = new Vector3();
		if (player != null) {
			pos= player.transform.position;
		} else {
			pos = enemy.transform.position;
		}
		
		pos.x = pos.x + 0.8f;

		Vector3 newPos = Camera.main.WorldToScreenPoint(pos);
		GameObject instance = Instantiate (HpDecreseIndicator, newPos, Quaternion.identity) as GameObject;

		if (player != null) {
			instance.name = player.name + "HpDecreseIndicator";
		} else {
			instance.name = enemy.name + "HpDecreseIndicator";
		}
			
		instance.transform.SetParent (canvas.transform);

		instance.transform.position = newPos;

		instance.SetActive(false);
		newHpDecreseIndicator = instance;
	}

	private void hideHPdereaseIndicator(){
		newHpDecreseIndicator.SetActive (false);
	}

	private void showHPdereaseIndicator(Vector3 pos,int hurt){
		
		pos.x = pos.x + 1.3f;
		pos.y = pos.y - 0.8f;
		Vector3 newPos = Camera.main.WorldToScreenPoint(pos);
		newHpDecreseIndicator.transform.position = newPos;
		newHpDecreseIndicator.GetComponent<Text> ().text = "-" + Convert.ToString(hurt);
		newHpDecreseIndicator.SetActive (true);
	}

	public void PlayerSetHPdereaseIndicator(Player playerObj, int hurt){
		Vector3 pos = playerObj.transform.position;
		showHPdereaseIndicator (pos,hurt);
		Invoke ("hideHPdereaseIndicator", 1.5f);

	}

	public void EnemySetHPdereaseIndicator(Enemy enemyObj, int hurt){
		Vector3 pos = enemyObj.transform.position;
		showHPdereaseIndicator (pos,hurt);
		Invoke ("hideHPdereaseIndicator", 1.5f);

	}

	public void clear(){
		Destroy (newHpDecreseIndicator);
	}

	// Use this for initialization
	void Start () {
		
		inItHpDecreseIndicator();


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
