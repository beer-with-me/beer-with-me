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
	public GameObject connectSetup_gmo;
	public GameObject linkDevice_gmo;
	public GameObject gameSetup_gmo;
	public GameObject gamePlay_gmo;
	public GameObject gameSettle_gmo;

	[HideInInspector] public bool is_Hoster;
	[HideInInspector] public int room_ID;

	[HideInInspector] public bool start_Here;
	[HideInInspector] public Vector2 start_Position;

	void Start(){
		is_Hoster = false;
		room_ID = -1;
		start_Here = false;
		SwitchPhases (Phases.ConnectSetup);
	}

//	void OnGUI(){
//		GUI.Label (new Rect(0, 0, 400, 400),"H: " + (Screen.height / Screen.dpi).ToString() + " ,W: " + (Screen.width / Screen.dpi).ToString());
//	}

	public void SwitchPhases(Phases toPhase){
		now_Phase = toPhase;
		connectSetup_gmo.SetActive (false);
		linkDevice_gmo.SetActive (false);
		gameSetup_gmo.SetActive (false);
		gamePlay_gmo.SetActive (false);
		gameSettle_gmo.SetActive (false);
		if(toPhase == Phases.ConnectSetup) 	connectSetup_gmo.SetActive (true);
		if(toPhase == Phases.LinkDevice) 	linkDevice_gmo.SetActive (true);
		if(toPhase == Phases.GameSetup) 	gameSetup_gmo.SetActive (true);
		if(toPhase == Phases.GamePlay)		gamePlay_gmo.SetActive (true);
		if(toPhase == Phases.GameSettle)	gameSettle_gmo.SetActive (true);
	}
}
