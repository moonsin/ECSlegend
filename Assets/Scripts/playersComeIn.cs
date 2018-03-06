using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;


public class playersComeIn: MonoBehaviour {

	public List<Player> players;

	private Text DiaLogPlayer;
	private Text DiaLogContent;
	private string[,] dialog = new string[4,4];
	private bool isShowing = false;
	private bool begin = false;
	private int showingNum = 0;


	void Start(){
		//47s
		Invoke ("init", 47f);

		//test code
		//GameManager.instance.turnIndicator.enabled = true;
		//Invoke ("HideturnIndicator", 1f);
	}

	private void setDiaLogBox(){
		
		dialog [0, 0] = "Ernest(Mage)";
		dialog [0, 1] = "I don't know why, but actually I have a bad felling.";

		dialog [1, 0] = "Tim(Knight)";
		dialog [1, 1] = "Dont worry Ernest, just keep walking, the village is not far from here. Southampton is a beatiful town.";

		dialog [2, 0] = "Ted(Archer)";
		dialog [2, 1] = "Stop moving! It's a trap! We are surrounded!";

		dialog [3, 0] = "Tim";
		dialog [3, 1] = "Don't be afraid, Just kill them all!";


	}

	public void HideturnIndicator(){
		GameManager.instance.turnIndicator.enabled = false;
		GameManager.instance.doingSetup = false;
	}


	public void init(){
		setDiaLogBox ();

		//Invoke ("showDiaLogBox", 1f);
		GameManager.instance.DiaLogImage.SetActive (true);
		DiaLogPlayer = GameObject.Find ("dialogPlayer").GetComponent<Text>();
		DiaLogContent = GameObject.Find ("dialogContent").GetComponent<Text>();

		DiaLogPlayer.text = dialog [showingNum, 0];
		DiaLogContent.text  = dialog [showingNum, 1];

		isShowing = true;
		begin = true;
		Invoke ("showTheDialog", 5f);
	}

	public void showTheDialog(){
		
		isShowing = false;
		showingNum += 1;
	}

	void Update () {
		if (!isShowing && showingNum <=3 && begin) {
			
			DiaLogPlayer.text  = dialog [showingNum, 0];
			DiaLogContent.text  = dialog [showingNum, 1];
			isShowing = true;
			Invoke ("showTheDialog", 6f);
		}
		if (showingNum == 4 && begin) {
			begin = false;
			GameManager.instance.DiaLogImage.SetActive (false);
			GameManager.instance.turnIndicator.enabled = true;
			Invoke ("HideturnIndicator", 1f);
		}
	}
}


