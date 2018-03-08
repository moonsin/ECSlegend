using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
	public bool firstClickMenu = false;
	public bool AlreadyfirstClickMenu = false;

	public bool firstClickMove = false;
	public bool AlreadyfirstClickMove = false;

	public bool firstClickKnightAttack = false;
	public bool AlreadyfirstClickKnightAttack = false;

	public bool firstClickBerserkerAttack = false;
	public bool AlreadyClickBerserkerAttack = false;

	public bool firstClickArcherAttack = false;
	public bool AlreadyfirstClickArcherAttack = false;

	public bool firstClickMageAttack = false;
	public bool AlreadyfirstClickMageAttack = false;

	public bool firstRest = false;
	public bool AlreadyfirstRest = false;

	public Text firstGuide;

	// Use this for initialization
	void Start () {
		
		//Invoke ("init", 71.5f);

		//test code
		//GameManager.instance.turnIndicator.enabled = true;
		//init();
	}

	public void init(){
		GameManager.instance.tutorialImage.SetActive (true);
	}
	void hideTutorial(){
		GameManager.instance.tutorialImage.SetActive (false);
	}

	// Update is called once per frame
	void Update () {
		if (firstClickMenu && !AlreadyfirstClickMenu) {
			AlreadyfirstClickMenu = true;
			firstGuide.text = "You have chosen a character to operate it. Looking at the menu, this character can ‘Move’, ‘Attack’ and ‘Rest’. After moving, the character cannot ‘Move’ any more in the same turn. So do ‘Attack’. The order of ‘Attack’ and ‘Move’ are not fixed. However, you should pay attention that once choose ‘Rest’, the character will finish its turn, which means it cannot act any more in the turn. Please click on move and attack before clicking rest.";
		}
		if (firstClickMove && !AlreadyfirstClickMove) {
			AlreadyfirstClickMove = true;
			firstGuide.text = "Now, you can see a range of grids where this character can move to in this turn. Every character can move two grids per turn. Moving will be effect by players, enemies and landform. Click on a grid where you want to move to.";
		}
		if (firstClickKnightAttack && !AlreadyfirstClickKnightAttack) {
			AlreadyfirstClickKnightAttack = true;
			firstGuide.text = "Now, you can see a range of grids, which is the attack range of Knights. Knights can attack enemies who are around him. You can click an enemy which is on his attack range and attack it.";
		}
		if (firstClickBerserkerAttack && !AlreadyClickBerserkerAttack) {
			AlreadyClickBerserkerAttack = true;
			firstGuide.text = "Now, you can see a range of grids, which are the attack ranges of berserkers. Berserkers’ attack range is small but its attack point is high. You can click an enemy which is on his attack range and attack it.";
		}
		if (firstClickArcherAttack && !AlreadyfirstClickArcherAttack) {
			AlreadyfirstClickArcherAttack = true;
			firstGuide.text = "Now, you can see a range of grids, which are the attack ranges of archers. The archers can attack the enemy far from him, but it cannot attack someone who is around him. You can click an enemy which is on his attack range and attack it.";
		}
		if (firstClickMageAttack && !AlreadyfirstClickMageAttack) {
			AlreadyfirstClickMageAttack = true;
			firstGuide.text = "Now, you can see a range of grids, which is the attack range of masters. When masters attack an enemy, it can attack other enemies around the enemy. You can click an enemy which is on his attack range and attack it.";
		}
		if (firstRest && !AlreadyfirstRest) {
			AlreadyfirstRest = true;
			firstGuide.text = "When you choose rest, the character will finish its turn, which means it cannot act any more in the turn.";
		}
		if (GameManager.instance.turnNum == 2) {
			firstGuide.text = "You aim is to defeat all enemies you can see. If all your characters are dead, the game will over.";
			Invoke ("hideTutorial",5f);
		}
	}
}
