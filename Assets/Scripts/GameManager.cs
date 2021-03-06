﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static bool playerMenuShowed = false;
	public BoardManager boardScript;
	public float backgroundStoryDelay = 2f;
	public float turnChangeDelay = 0.5f;
	public bool enemyTurnShowed = false;
	public bool playerTurnShowed = false;
	public bool beginingAniFinished = false;
	public GameObject DiaLogImage;
	public GameObject tutorialImage;
	public bool enemyObjectsSet = false;
	public bool playerObjectsSet = false;

	private int busyEnemyIndex = 0;
	public int turnNum = 1;

	public List<Player> players;
	public List<Enemy> enemies;
	public Text turnIndicator;
	public Text turnText;
	public Text gameOver;
	public Text Victory;
	public bool victory = false;
	public Text endTitle;
	public Text MemoryOf;
	public Text Finaltitle;
	private Text story;
	private Text title;
	public Text skipBackGroundStory;
	private Button PlayerMenu_MoveButton;
	private Button PlayerMenu_AttackButton;
	private Button PlayerMenu_RestButton;
	private Text dramaStory;
	private GameObject BackgroundStoryImage;
	private GameObject PlayerMenu;
	public bool doingSetup = false;
	private  AudioSource m_MyAudioSource;
	private  AudioSource endMusic;
	private TutorialManager tutorial;
	private bool FinalAniAlreadyPlayed = false;
	private bool timeplyMoved = false;
	private bool hideskipBackGroundStoryisShowed = false;

	public bool isMenuShowing(){
		return playerMenuShowed;
	}

	public List<Player> GetAllPlayers(){
		return players;
	}

	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		enemies = new List<Enemy> ();
		players = new List<Player> ();
		boardScript = GetComponent<BoardManager> ();
		tutorial = GetComponent<TutorialManager> ();
		InitGame ();
	}

	public void AddPlayerToList (Player script){
		players.Add (script);
	}

	public void AddEnemyToList(Enemy script){
		enemies.Add (script);
	}

	private void HideBackgroundImage(){
		BackgroundStoryImage.SetActive (false);
		StopBackGroundMusic ();
		dramaStory.enabled = false;
		if (!beginingAniFinished) {
			this.GetComponent<playersComeIn> ().init ();
		}
		beginingAniFinished = true;

	}

	public void HideturnIndicator(){
		turnIndicator.enabled = false;
		doingSetup = false;
	}

	public void HidePlayerMenu(){
		PlayerMenu.SetActive (false);
		playerMenuShowed = false;
	}

	public void ShowPlayerMenu(Player player){
		Vector3 pos = player.transform.position;
		Vector3 newPos = Camera.main.WorldToScreenPoint(new Vector3 (pos.x+1, pos.y, -0.5f));
		PlayerMenu.transform.position = newPos;

		//PlayerMenu_MoveButton.enabled = player.hasMoved ? false : true;
		if (player.hasMoved) {
			PlayerMenu_MoveButton.GetComponent<Image> ().color = Color.grey; 
		} else {
			PlayerMenu_MoveButton.GetComponent<Image> ().color = Color.blue;
		}
		if (player.hasAttacked) {
			PlayerMenu_AttackButton.GetComponent<Image> ().color = Color.grey; 
		} else {
			PlayerMenu_AttackButton.GetComponent<Image> ().color = Color.blue;
		}
		PlayerMenu_RestButton.GetComponent<Image> ().color = Color.blue;
		PlayerMenu.SetActive (true);
		playerMenuShowed = true;

		tutorial.firstClickMenu = true;


	}

	private void showturnIndicator(){
		turnIndicator.enabled = true;
		Invoke ("HideturnIndicator", turnChangeDelay);
		enemyTurnShowed = true;
		playerTurnShowed = false;
	}
	private void showturnIndicator2(){
		turnIndicator.text = "Players' turn!";
		turnIndicator.enabled = true;
		Invoke ("HideturnIndicator", turnChangeDelay);
	}
		
	private void smaller(){
		title.fontSize -=1;
	}

	private void showDramaStory(){
		title.enabled = false;
		dramaStory.enabled = true;
		//boardScript.LayoutObjectAtRandom (boardScript.treeTiles,boardScript.treeCount.minimum, boardScript.treeCount.maximum);

		Invoke ("HideBackgroundImage", 37f);
	}

	private void showTitle(){
		story.enabled = false;
		title.enabled = true;
		PlayBackGroundMusic ();
		Invoke ("showDramaStory", 8f);
	}

	private void InitBackGround(){
		endTitle.enabled = false;
		MemoryOf.enabled = false;
		Finaltitle.enabled = false;
		title.enabled = false;
		dramaStory.enabled = false;
		skipBackGroundStory.enabled = false;
		BackgroundStoryImage.SetActive (true);
		Invoke ("showTitle", backgroundStoryDelay);

	}

	private void PlayBackGroundMusic(){
		m_MyAudioSource.Play ();
	}

	private void StopBackGroundMusic(){
		m_MyAudioSource.Stop ();
	}

	private void PlayEndTitleMusic(){
		endMusic.Play ();
	}

	void InitGame(){

		m_MyAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
		endMusic = GameObject.Find("GameendTitle").GetComponent<AudioSource>();

		doingSetup = true;
		BackgroundStoryImage = GameObject.Find ("backGroundStory");
		PlayerMenu = GameObject.Find ("playerMenu");
		PlayerMenu_MoveButton = GameObject.Find ("MoveButton").GetComponent<Button> ();
		PlayerMenu_AttackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		PlayerMenu_RestButton = GameObject.Find ("RestButton").GetComponent<Button> ();
		HidePlayerMenu ();

		turnIndicator = GameObject.Find ("turnIndicator").GetComponent<Text>();
		turnText = GameObject.Find ("turnText").GetComponent<Text>();
		gameOver = GameObject.Find ("gameOver").GetComponent<Text>();

		Victory = GameObject.Find ("Victory").GetComponent<Text>();
		story = GameObject.Find ("Story").GetComponent<Text>();
		title = GameObject.Find ("Title").GetComponent<Text>();

		endTitle = GameObject.Find ("endTitle").GetComponent<Text>();
		MemoryOf = GameObject.Find ("MemoryOf").GetComponent<Text>();
		Finaltitle =  GameObject.Find ("Finaltitle").GetComponent<Text>();
		skipBackGroundStory =  GameObject.Find ("skipBackGroundStory").GetComponent<Text>();

		dramaStory = GameObject.Find ("dramaStory").GetComponent<Text>();
		DiaLogImage = GameObject.Find ("dialogBox");
		tutorialImage = GameObject.Find ("tutorial");



		turnIndicator.enabled = false;
		gameOver.enabled = false;
		Victory.enabled = false;



		DiaLogImage.SetActive (false);
		tutorialImage.SetActive(false);

		InitBackGround();

		//test code
		//HideBackgroundImage();

		boardScript.SetupScene ();

	}

	public bool IsPlayTurnFinish(){
		
		for (int i = 0; i < players.Count; i++) {
			if (!players [i].GetTurnFinish ()) {
				return false;
			}
		}
		return true;
	}

	public bool IsEnemyTurnFinish(){
		
		for (int i = 0; i < enemies.Count; i++) {
			if (!enemies [i].GetTurnFinish ()) {
				return false;
			}
		}
		return true;
	}

	void OperateEnemy(Enemy enemy){
		//turnText.text = Convert.ToString(enemy.hasMoved);
			if (!enemy.hasMoved) {
				if(!timeplyMoved){
					timeplyMoved = true;
					enemy.MoveEnemy ();
				}
			}else {
				enemy.EnemyAttack ();
				busyEnemyIndex += 1;
				timeplyMoved = false;
			}

	}


	void newTurn(){
		turnNum += 1;
		turnText.text = "Turn: " + Convert.ToString (turnNum);
		playerTurnShowed = true;

		for (int i = 0; i < players.Count; i++) {
			players [i].turnFinished = false;
			players [i].hasMoved = false;
			players [i].hasAttacked = false;
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].turnFinished = false;
			enemies [i].hasMoved = false;
			enemies [i].hasAttacked = false;
		}
		enemyTurnShowed = false;


	}

	public bool isAllPlayersdead(){
		if (players.Count != 0) {
			playerObjectsSet = true;
			return false;
		} else if (playerObjectsSet) {
			return true;
		}else{
			return false;
		}
	}

	public bool isAllEnemiesdead(){
		if (enemies.Count != 0) {
			enemyObjectsSet = true;
			return false;
		} else if (enemyObjectsSet) {
			return true;
		}else{
			return false;
		}
	}

	private void gameQuit(){
		Application.Quit ();
	}

	private void hideMemoryOf(){
		MemoryOf.enabled = false;
		Finaltitle.enabled = true;
		Invoke ("gameQuit", 80f);
	}

	private void hideEndTitle(){
		endTitle.enabled = false;
		MemoryOf.enabled = true;
		Invoke ("hideMemoryOf", 5f);
	}

	private void FinalThanksPlay(){

		GameObject[] PlayerList  = GameObject.FindGameObjectsWithTag ("Player");
		foreach (GameObject element in PlayerList) {
			Destroy (element);
		}

		GameObject[] HpIndicatorInstances  = GameObject.FindGameObjectsWithTag ("HpIndicator");
		print (HpIndicatorInstances);
		foreach (GameObject element in HpIndicatorInstances) {
			Destroy (element);
		}


		BackgroundStoryImage.SetActive (true);
		PlayEndTitleMusic ();
		endTitle.enabled = true;
		Invoke ("hideEndTitle", 5f);
	}

	private void hideskipBackGroundStory(){
		skipBackGroundStory.enabled = false;
		hideskipBackGroundStoryisShowed = false;
	}
	// Update is called once per frame
	void Update () {

		//turnText.text = Convert.ToString(busyEnemyIndex);

		if(!beginingAniFinished){
			if (Input.anyKeyDown) {
				skipBackGroundStory.enabled = true;
				if(Input.GetKeyDown("space") && hideskipBackGroundStoryisShowed){
					HideBackgroundImage ();
				}
				else {
					hideskipBackGroundStoryisShowed = true;
					Invoke ("hideskipBackGroundStory", 2.5f);
				}

			}
		}

		if (doingSetup) {
			return;
		}

		if (IsPlayTurnFinish () && !enemyTurnShowed) {
			turnIndicator.text = "Enemies' turn!";
			showturnIndicator ();
		}


		if (IsPlayTurnFinish () && !IsEnemyTurnFinish() ) {
			if (busyEnemyIndex != 0) {
				if (busyEnemyIndex < enemies.Count && enemies [busyEnemyIndex - 1].turnFinished) {
					OperateEnemy (enemies [busyEnemyIndex]);
				}
			} else {
				if (busyEnemyIndex < enemies.Count) {
					OperateEnemy (enemies [busyEnemyIndex]);
				}
			}
		}



		if (isAllPlayersdead()) {
			gameOver.enabled = true;
			Invoke ("gameQuit", 3.5f);
			return;
		}
		if (isAllEnemiesdead()) {
			victory = true;

			GameObject[] HpIndicatorInstances  = GameObject.FindGameObjectsWithTag ("HpIndicator");
			print (HpIndicatorInstances);
			foreach (GameObject element in HpIndicatorInstances) {
				Destroy (element);
			}
			print (HpIndicatorInstances);
			Victory.enabled = true;
			if(!FinalAniAlreadyPlayed){
				FinalAniAlreadyPlayed = true;
				Invoke ("FinalThanksPlay", 3.5f);
			}
			return;
		}

		if (IsPlayTurnFinish () && IsEnemyTurnFinish ()&& !playerTurnShowed) {
			busyEnemyIndex = 0;
			newTurn ();
			Invoke ("showturnIndicator2", 1.5f);
		}
		
	}
}
