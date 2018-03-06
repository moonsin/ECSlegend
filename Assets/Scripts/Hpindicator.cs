using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Hpindicator : MonoBehaviour {
	public Canvas canvas;
	public GameObject HPindicator;
	public Player player;

	// Use this for initialization
	protected void inItHpIndicator (){

		if(player.hp <=0){
			return;
		}

		Vector3 pos = player.transform.position;
		pos.y = pos.y + 0.8f;
		Vector3 newPos = Camera.main.WorldToScreenPoint(pos);
		GameObject instance = Instantiate (HPindicator, newPos, Quaternion.identity) as GameObject;
		instance.name = player.name + "HpIndicator";
		instance.transform.SetParent (canvas.transform);
		instance.transform.position = newPos;

		instance.GetComponent<Text> ().text = "HP:" + Convert.ToString(player.hp) + "/" + Convert.ToString(player.fullHP);


	}


	void Start () {
		//inItHpIndicator ();
	
	}
	
	// Update is called once per frame
	void Update () {
		Destroy (GameObject.Find (player.name + "HpIndicator"));
		print (player.isDead);
		if (GameManager.instance.beginingAniFinished) {
			inItHpIndicator ();
		}

	}
}
