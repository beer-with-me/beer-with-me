using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GamePlay_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	private int serverReceiveIndex;
	private bool onReceive;
	private Packet receivePacket;

	public Swipe swipe;

	public bool isPlaying = false;
	public float lastDistance = 0.0f;
	public bool isLeavingTable = false;

	public GameObject beer_prefab;
	private GameObject beer;

	// when the phase begin
	void OnEnable () {
		isPlaying = true;
		serverReceiveIndex = networkController.AddSubscriptor (new Subscriptor(OnReceive, new M2C_Command[2]{M2C_Command.M2C_CROSS, M2C_Command.M2C_SCORE}));
		if (gameController.start_Here) {
			beer = Instantiate (beer_prefab, gameController.start_Position, Quaternion.Euler (new Vector3 (-90, 0, 0)));
			beer.transform.parent = GameObject.Find ("GamePlayPhase").transform;
			swipe = beer.GetComponent<Swipe> ();
		}
	}

	void OnDisable () {
		isPlaying = false;
		networkController.RemoveSubscriptor (serverReceiveIndex);
	}

	void Update(){
		if (onReceive) {
			AnalysisReceive (receivePacket);
			onReceive = false;
		}
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
		networkController.SendToServer (new Packet (gameController.version, C2M_Command.C2M_BEER_STOP, new int[1]{(int) lastDistance * 1000}));
	}

	private void Cross(int dirKey) {
		networkController.SendToServer (new Packet (gameController.version, C2M_Command.C2M_CROSS, new int[3]{dirKey, 1, (int) (lastDistance * 1000)}, new float[3]{beer.GetComponent<Rigidbody>().velocity.x, beer.GetComponent<Rigidbody>().velocity.y, beer.GetComponent<Rigidbody>().velocity.z}));
		Destroy (beer);
	}

	public void OnReceive(Packet packet) {
		onReceive = true;
		receivePacket = packet;
	}

	public void AnalysisReceive(Packet packet){
		Debug.Log ("GamePlay receive");
		switch (packet.M2C_command) {
		case M2C_Command.M2C_CROSS:
			M2C_Cross (packet);
			break;
		default:
			break;
		}
	}

	public void M2C_Cross(Packet packet){
		
	}
}
