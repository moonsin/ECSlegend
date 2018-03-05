using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundStory : MonoBehaviour {


	private Text story;
	private Text title;
	private Text dramaStory;
	private GameObject BackgroundStoryImage;
	public float backgroundStoryDelay = 0.5f;
	private  AudioSource m_MyAudioSource;

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

	public void HideBackgroundImage(){
		BackgroundStoryImage.SetActive (false);
		StopBackGroundMusic ();
		//Invoke ("HideturnIndicator", 0.5f);
	}


	public void InitBackGround(){
		
		m_MyAudioSource = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
		BackgroundStoryImage = GameObject.Find ("backGroundStory");
		story = GameObject.Find ("Story").GetComponent<Text>();
		title = GameObject.Find ("Title").GetComponent<Text>();
		dramaStory = GameObject.Find ("dramaStory").GetComponent<Text>();


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


	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
