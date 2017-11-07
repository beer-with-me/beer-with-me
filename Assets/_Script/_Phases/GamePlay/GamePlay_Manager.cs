using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlay_Manager : MonoBehaviour {
	public bool isPlaying = false;
	public float lastDistance = 0.0f;
	public bool isLeavingTable = false;
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

	public void Stop() {
		if (!isLeavingTable) {
			Debug.Log ("Game Over");
			BeerStop ();
		}
	}

	public void EnterEdge(int dirKey) {
		Debug.Log ("Edge_" + dirKey.ToString() + " entered.");
		Cross (dirKey);
	}

	private void BeerStop() {
		networkController.SendToServer (new Pocket (gameController.version, C2M_Command.C2M_BEER_STOP, new int[1]{(int) lastDistance * 1000}));
	}

	private void Cross(int dirKey) {
		networkController.SendToServer (new Pocket (gameController.version, C2M_Command.C2M_CROSS, new int[6]{dirKey, 1, 1, 1, 1, (int) lastDistance * 1000}));
	}

	private void OnReceive(IAsyncResult AR) {
		Debug.Log (AR.ToString ());
	}
}
