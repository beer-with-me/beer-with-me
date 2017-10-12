using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ScreenName{
	MapConstructor,
	MetaGame
}

public class GameController : MonoBehaviour {
	public GameObject square;
	public Transform mapBlocksFolder;

	public GameObject mapStructorScreen;
	public GameObject MetaGameScreen;

	void Start(){

		SwitchScreen (ScreenName.MapConstructor);
	}

	void OnGUI(){
		GUI.Label (new Rect(0, 0, 400, 400),"H: " + (Screen.height / Screen.dpi).ToString() + " ,W: " + (Screen.width / Screen.dpi).ToString());
	}

	public void SwitchScreen(ScreenName screenName){
		switch (screenName) {
		case ScreenName.MapConstructor:
			mapStructorScreen.SetActive (true);
			MetaGameScreen.SetActive (false);

			break;
		case ScreenName.MetaGame:
			mapStructorScreen.SetActive (false);
			MetaGameScreen.SetActive (true);

			break;
		}
	}
}
