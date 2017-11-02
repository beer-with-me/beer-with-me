using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlay_Manager : MonoBehaviour {
	public bool isPlaying = false;
	public float lastDistance = 0.0f;
	public GameController gameController;
	public NetworkController networkController;

	private int serverReceiveIndex;

	// when the phase begin
	void OnEnable () {
		isPlaying = true;
		serverReceiveIndex = networkController.AddReceiveListener (new AsyncCallback(OnReceive));
	}

	void OnDisable () {
		isPlaying = false;
		networkController.RemoveReceiveListener (serverReceiveIndex);
	}

	public void LeaveTable(int dirKey) {
		Debug.Log ("Edge_" + dirKey.ToString() + " entered.");
		networkController.SendToServer (new Pocket (gameController.version, C2M_Command.C2M_CROSS, new int[6]{dirKey, 1, 1, 1, 1, (int) lastDistance * 1000}));
	}

	private void OnReceive(IAsyncResult AR) {
		Debug.Log (AR.ToString ());
	}
}
