using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public BoardManager boardScript;
	public float backgroundStoryDelay = 2f;
	public float turnChangeDelay = 1f;
	public bool alreadyshowedTurnChange = false;

	private List<Player> players;
	private Text turnIndicator;
	private GameObject BackgroundStoryImage;
	private bool doingSetup = false;


	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);

		boardScript = GetComponent<BoardManager> ();
		InitGame ();
	}

	public void AddPlayerToList (Player script){
		players.Add (script);
	}

	private void HideBackgroundImage(){
		BackgroundStoryImage.SetActive (false);
		Invoke ("HideturnIndicator", turnChangeDelay);
	}

	private void HideturnIndicator(){
		turnIndicator.enabled = false;
		doingSetup = false;
	}

	private void showturnIndicator(){
		turnIndicator.enabled = true;
		Invoke ("HideturnIndicator", turnChangeDelay);
		alreadyshowedTurnChange = true;
	}

	private void showTurnIndicator(string who){
		
	}
	void InitGame(){

		doingSetup = true;
		BackgroundStoryImage = GameObject.Find ("backGroundStory");
		turnIndicator = GameObject.Find ("turnIndicator").GetComponent<Text>();

		BackgroundStoryImage.SetActive (true);
		Invoke ("HideBackgroundImage", backgroundStoryDelay);

		players = new List<Player>();
		boardScript.SetupScene ();
	}

	bool IsPlayTurnFinish(){
		Player[] players = GameObject.FindObjectsOfType <Player>();
		for (int i = 0; i < players.Length; i++) {
			if (!players [i].GetTurnFinish ()) {
				return false;
			}
		}
		return true;
	}


	// Update is called once per frame
	void Update () {
		if (doingSetup) {
			return;
		}
		if (IsPlayTurnFinish () && !alreadyshowedTurnChange) {
			//print ("Players Turn Finish");
			turnIndicator.text = "enemy's turn!";
			showturnIndicator ();
		}
	}
}
