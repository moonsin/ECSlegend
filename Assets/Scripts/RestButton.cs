using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestButton : MonoBehaviour {

	void Start () {
		Button btn = this.GetComponent<Button> ();
		btn.onClick.AddListener (OnClick);
	}

	private void OnClick(){
		print ("Button Clicked. ClickHandler.");
		Player.buttonChoice = "rest"; 
	}

}
