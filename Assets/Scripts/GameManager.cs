using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GameManager : MonoBehaviour {

	public static GameManager instance = null;
	public static bool playerMenuShowed = false;
	public BoardManager boardScript;
	public float backgroundStoryDelay = 2f;
	public float turnChangeDelay = 0.5f;
	public bool enemyTurnShowed = false;
	public bool playerTurnShowed = false;


	public List<Player> players;
	public List<Enemy> enemies;
	private Text turnIndicator;
	private Text story;
	private Text title;
	private Button PlayerMenu_MoveButton;
	private Button PlayerMenu_AttackButton;
	private Button PlayerMenu_RestButton;
	private Text dramaStory;
	private GameObject BackgroundStoryImage;
	private GameObject PlayerMenu;
	private bool doingSetup = false;
	private  AudioSource m_MyAudioSource;

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
		Invoke ("HideturnIndicator", turnChangeDelay);
	}

	private void HideturnIndicator(){
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

	private void showTurnIndicator(string who){

	}

	private void smaller(){
		title.fontSize -=1;
	}

	private void showDramaStory(){
		title.enabled = false;
		dramaStory.enabled = true;
		Invoke ("HideBackgroundImage", 37f);
	}

	private void showTitle(){
		story.enabled = false;
		title.enabled = true;
		Invoke ("showDramaStory", 8f);
	}

	private void InitBackGround(){
		title.enabled = false;
		dramaStory.enabled = false;
		BackgroundStoryImage.SetActive (true);
		Invoke ("showTitle", backgroundStoryDelay);
		Invoke ("PlayBackGroundMusic", backgroundStoryDelay);

	}

	private void PlayBackGroundMusic(){
		m_MyAudioSource.Play ();
	}

	private void StopBackGroundMusic(){
		m_MyAudioSource.Stop ();
	}

	void InitGame(){

		m_MyAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
		doingSetup = true;
		BackgroundStoryImage = GameObject.Find ("backGroundStory");
		PlayerMenu = GameObject.Find ("playerMenu");
		PlayerMenu_MoveButton = GameObject.Find ("MoveButton").GetComponent<Button> ();
		PlayerMenu_AttackButton = GameObject.Find ("AttackButton").GetComponent<Button> ();
		PlayerMenu_RestButton = GameObject.Find ("RestButton").GetComponent<Button> ();
		HidePlayerMenu ();

		turnIndicator = GameObject.Find ("turnIndicator").GetComponent<Text>();
		story = GameObject.Find ("Story").GetComponent<Text>();
		title = GameObject.Find ("Title").GetComponent<Text>();
		dramaStory = GameObject.Find ("dramaStory").GetComponent<Text>();

		//Vector3 newPost = Camera.main.WorldToScreenPoint(new Vector3 (2.5f, 2.5f -0.5f));
		//PlayerMenu.transform.position = newPost;


		//InitBackGround();

		//test code
		HideBackgroundImage();

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
		if (!enemy.GetTurnFinish() && !enemy.GetIsDead()) {
			enemy.MoveEnemy ();
			enemy.EnemyAttack ();
			}
	}


	void newTurn(){
		
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

	// Update is called once per frame
	void Update () {
		if (doingSetup) {
			return;
		}
		if (IsPlayTurnFinish () && !enemyTurnShowed) {
			turnIndicator.text = "Enemies' turn!";
			showturnIndicator ();
		}
		if (IsPlayTurnFinish () && !IsEnemyTurnFinish()) {
			for (int i = 0; i < enemies.Count; i++) {
				OperateEnemy (enemies[i]);
			}
		}
		if (IsPlayTurnFinish () && IsEnemyTurnFinish ()&& !playerTurnShowed) {
			
			newTurn ();
			Invoke ("showturnIndicator2", 1.5f);
		}

	}
}
