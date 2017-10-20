﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start_buttom : MonoBehaviour {
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	void OnMouseUp(){
		StartCoroutine (Start_game ());
	}

	IEnumerator Start_game(){
		yield return new WaitForSeconds (0.01f);
		gameController.SwitchScreen (ScreenName.MetaGame);
	}
}
