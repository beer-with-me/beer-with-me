﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;

	// when the phase begin
	void OnEnable () {
		
	}

	public void LeaveTable(int dirKey) {
		Debug.Log ("Edge_" + dirKey.ToString() + " entered.");
		networkController.SendToServer (new Pocket (gameController.version, C2M_Command.C2M_CROSS, new int[6]{dirKey, 1, 1, 1, 1, 1}));
	}
}
