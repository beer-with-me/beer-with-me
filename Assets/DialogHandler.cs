﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHandler : MonoBehaviour {

	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = gameObject.GetComponent<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Confirm () {
		gameController.dialog_Delegate (true);
		gameController.DialogCanvas.SetActive (false);
	}

	public void Cancel () {
		gameController.dialog_Delegate (false);
		gameController.DialogCanvas.SetActive (false);
	}
}
