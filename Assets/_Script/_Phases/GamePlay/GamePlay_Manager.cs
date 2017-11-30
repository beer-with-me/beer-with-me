using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamePlay_Manager : MonoBehaviour {

	private List<Vector3> directionMap = new List<Vector3>(new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f),
		new Vector3(3.3f, 0.0f, 2.5f), new Vector3(3.3f, 0.0f, -2.5f), new Vector3(0.0f, 0.0f, -5.0f),
		new Vector3(-3.3f, 0.0f, -2.5f), new Vector3(-3.3f, 0.0f, 2.5f), new Vector3(0.0f, 0.0f, 5.0f) });
	private List<Vector3> offsetMap = new List<Vector3>(new Vector3[] { new Vector3(0.0f, 0.0f, 0.0f),
		new Vector3(0.0f, 0.0f, 1.25f), new Vector3(0.0f, 0.0f, 1.25f), new Vector3(1.5f, 0.0f, 0.0f),
		new Vector3(0.0f, 0.0f, 2.5f), new Vector3(0.0f, 0.0f, 2.5f), new Vector3(1.5f, 0.0f, 0.0f) });
	
	public GameController gameController;
	public NetworkController networkController;
	private int serverReceiveIndex;
	private bool onReceive;
	private Packet receivePacket;

	public Swipe swipe;

	private int multiple = 20;
	public bool isPlaying = false;
	public float lastDistance = 0.0f;
	public bool isLeavingTable = false;

	public GameObject beer_prefab;
	public GameObject table;
	public Camera mainCamera;
	private GameObject beer;

	// when the phase begin
	void OnEnable () {
		if (gameController.width_length != 0) {
			table.transform.localScale = new Vector3 (gameController.width_length, 1, gameController.height_length);
			mainCamera.orthographicSize = gameController.width_length * gameController.height_length / 2;
		}
			
		multiple = 20;
		isPlaying = true;
		lastDistance = 0.0f;
		isLeavingTable = false;
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

	public int GetDistance(float distance) {
		return (int)(lastDistance * 100);
	}

	public void EnterEdge(int dirKey) {
		Debug.Log ("Edge_" + dirKey.ToString() + " entered.");
		Cross (dirKey);
	}

	private void BeerStop() {
		networkController.SendToServer (new Packet (gameController.version, C2M_Command.C2M_BEER_STOP, new int[1]{this.GetDistance(lastDistance)}));
	}

	private void Cross(int dirKey) {
		Rigidbody rb = beer.GetComponent<Rigidbody> ();
		networkController.SendToServer (new Packet (gameController.version, C2M_Command.C2M_CROSS, new int[3]{dirKey, 1, this.GetDistance(lastDistance)}, new float[3]{rb.velocity.x, 0, rb.velocity.z}));
		Destroy (beer);
	}

	public void OnReceive(Packet packet) {
		onReceive = true;
		receivePacket = packet;
	}

	public void AnalysisReceive(Packet packet) {
		Debug.Log ("GamePlay receive");
		switch (packet.M2C_command) {
		case M2C_Command.M2C_CROSS:
			M2C_Cross (packet);
			break;
		case M2C_Command.M2C_SCORE:
			M2C_Score (packet);
			break;
		default:
			break;
		}
	}

	public void M2C_Cross(Packet packet){
		int dirKey = packet.datas [0];
		int offset = packet.datas [1];
		int distance = packet.datas [2];
		float x = packet.f_datas [0];
		// float y = packet.f_datas [1];
		float z = packet.f_datas [2];

		beer = Instantiate (beer_prefab, directionMap[dirKey] + offsetMap[dirKey] * offset, Quaternion.Euler (new Vector3 (-90, 0, 0)));
		beer.GetComponent<Rigidbody> ().AddForce (new Vector3 (x * multiple, 0.0f, z * multiple));

		this.lastDistance += distance;
//		Debug.Log (dirKey.ToString ());
//		Debug.Log (offset.ToString ());
//		Debug.Log (distance.ToString ());
//		Debug.Log (x.ToString ());
//		Debug.Log (y.ToString ());
//		Debug.Log (z.ToString ());
	}

	public void OnPressOk(bool ok) {
		Destroy (beer);
		if (ok) {
			gameController.SwitchPhases (Phases.LinkDevice);
		} else {
			SceneManager.LoadScene(0);
		}

	}

	public void M2C_Score(Packet packet){
		gameController.Start_Dialog (OnPressOk, "Score", packet.datas [1].ToString (), 2);
	}
}
