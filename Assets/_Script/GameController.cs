using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public enum Phases{
	ConnectSetup,
	LinkDevice,
	GameSetup,
	GamePlay,
	GameSettle
}

public class GameController : MonoBehaviour {
	public NetworkController networkController;

	public int version = 1;
	public Phases now_Phase;
	public GameObject ConnectSetup_gmo;
	public GameObject LinkDevice_gmo;
	public GameObject GameSetup_gmo;
	public GameObject GamePlay_gmo;
	public GameObject GameSettle_gmo;

	public bool is_hoster;
	public int room_ID;

	void Start(){
		is_hoster = false;
		room_ID = -1;
		SwitchPhases (Phases.ConnectSetup);
	}

//	void OnGUI(){
//		GUI.Label (new Rect(0, 0, 400, 400),"H: " + (Screen.height / Screen.dpi).ToString() + " ,W: " + (Screen.width / Screen.dpi).ToString());
//	}

	public void SwitchPhases(Phases toPhase){
		now_Phase = toPhase;
		ConnectSetup_gmo.SetActive (false);
		LinkDevice_gmo.SetActive (false);
		GameSetup_gmo.SetActive (false);
		GamePlay_gmo.SetActive (false);
		GameSettle_gmo.SetActive (false);
		if(toPhase == Phases.ConnectSetup) 	ConnectSetup_gmo.SetActive (true);
		if(toPhase == Phases.LinkDevice) 	LinkDevice_gmo.SetActive (true);
		if(toPhase == Phases.GameSetup) 	GameSetup_gmo.SetActive (true);
		if(toPhase == Phases.GamePlay)		GamePlay_gmo.SetActive (true);
		if(toPhase == Phases.GameSettle)	GameSettle_gmo.SetActive (true);
	}
}
