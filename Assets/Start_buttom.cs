using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_buttom : MonoBehaviour {
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	void OnMouseDown(){
		gameController.SwitchScreen (ScreenName.MetaGame);
	}
}
