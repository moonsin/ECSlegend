using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Hpindicator_Enemy : MonoBehaviour {
	public Canvas canvas;
	public GameObject HPindicator;
	public GameObject newHPindicator;
	public Enemy enemy;

	// Use this for initialization
	protected void inItHpIndicator (){

		//Vector3 newPos = Camera.main.WorldToScreenPoint(pos);
		//print (newPos);
		//Vector3 newPos = Camera.main.ScreenToViewportPoint (this.pos);
		if(enemy.hp <=0){
			return;
		}
		Vector3 pos = enemy.transform.position;
		pos.y = pos.y + 0.8f;
		Vector3 newPos = Camera.main.WorldToScreenPoint(pos);
		GameObject instance = Instantiate (HPindicator, newPos, Quaternion.identity) as GameObject;
		instance.name = enemy.name + "HpIndicator";
		instance.transform.SetParent (canvas.transform);
		instance.transform.position = newPos;

		instance.GetComponent<Text> ().text = "HP:" + Convert.ToString(enemy.hp) + "/" + Convert.ToString(enemy.fullHP);
		newHPindicator = instance;

	}

	public void Clear(){
		Destroy (newHPindicator);
	}

	void Start () {
		inItHpIndicator ();

	}

	// Update is called once per frame
	void Update () {
		Destroy (newHPindicator);
		if (GameManager.instance.beginingAniFinished) {
			inItHpIndicator ();
		}
	}
}
