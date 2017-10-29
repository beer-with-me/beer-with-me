using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlay_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;

	private int serverReceiveIndex;

	// when the phase begin
	void OnEnable () {
		serverReceiveIndex = networkController.AddReceiveListener (new AsyncCallback(OnReceive));
	}

	void OnDisable () {
		networkController.RemoveReceiveListener (serverReceiveIndex);
	}

	public void LeaveTable(int dirKey) {
		Debug.Log ("Edge_" + dirKey.ToString() + " entered.");
		networkController.SendToServer (new Pocket (gameController.version, C2M_Command.C2M_CROSS, new int[6]{dirKey, 1, 1, 1, 1, 1}));
	}

	private void OnReceive(IAsyncResult AR) {
		Debug.Log (AR.ToString ());
	}
}
