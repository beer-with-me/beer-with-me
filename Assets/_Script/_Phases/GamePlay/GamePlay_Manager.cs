using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GamePlay_Manager : MonoBehaviour {

	private List<Vector3> directionMap;
	private List<Vector3> offsetMap;
	
	public GameController gameController;
	public NetworkController networkController;
	private int serverReceiveIndex;
	private bool onReceive;
	private Packet receivePacket;

	public Swipe swipe;

	public int forceMultiplication = 0;
	public bool isPlaying = false;
	public float lastDistance = 0.0f;
	public bool isLeavingTable = false;

	public GameObject beer_prefab;
	public GameObject table;
	public Camera mainCamera;
	private GameObject beer;
	private GameObject parent;
	private float tableWidth;
	private float tableHeight;

	public Transform objects_Folder;
	public GameObject square_gmo;

	// when the phase begin
	void OnEnable () {
		Delete_all_objects (new string[2]{"Beer", "Item"});
		mainCamera.orthographicSize = gameController.width_length * gameController.height_length / 2;
		table.transform.localScale = new Vector3 (gameController.width_length, 1, gameController.height_length);
		tableWidth = table.GetComponent<MeshRenderer> ().bounds.size.x;
		tableHeight = table.GetComponent<MeshRenderer> ().bounds.size.z;
		float xAdjust = tableWidth / 2;
		float zAdjust = tableHeight / 2;
		directionMap = new List<Vector3>(new Vector3[] { new Vector3(0, 0, 0),
			new Vector3(xAdjust, 0, zAdjust / 2), new Vector3(xAdjust, 0, -zAdjust / 2), new Vector3(0, 0, -zAdjust),
			new Vector3(-xAdjust, 0, -zAdjust / 2), new Vector3(-xAdjust, 0, zAdjust / 2), new Vector3(0, 0, zAdjust) });
		offsetMap = new List<Vector3>(new Vector3[] { new Vector3(0, 0, 0),
			new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 0),
			new Vector3(0, 0, 1), new Vector3(0, 0, 1), new Vector3(1, 0, 0) });

		parent = GameObject.Find ("GamePlayPhase");
		isPlaying = true;
		lastDistance = 0.0f;
		forceMultiplication = 20;
		isLeavingTable = false;
		serverReceiveIndex = networkController.AddSubscriptor (new Subscriptor(OnReceive, new M2C_Command[2]{M2C_Command.M2C_CROSS, M2C_Command.M2C_SCORE}));

		Init_Table ();
	}

	void Init_Table(){
		if (gameController.start_Here) {
			beer = Instantiate (beer_prefab, gameController.start_Position, Quaternion.Euler (new Vector3 (-90, 0, 0)), parent.transform);
			RepopulateBeer (beer);
			swipe = beer.GetComponent<Swipe> ();
		}
		if (gameController.square_Here) {
			Instantiate (square_gmo, new Vector3(0, 20, 0), Quaternion.identity, objects_Folder);
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
		return (int)(lastDistance * 10);
	}

	public void EnterEdge(int dirKey, float offset) {
		Debug.Log ("Edge_" + dirKey.ToString() + " entered from offset " + offset.ToString());
		Cross (dirKey, offset);
	}

	private void BeerStop() {
		networkController.SendToServer (new Packet (gameController.version, C2M_Command.C2M_BEER_STOP, new int[1]{this.GetDistance(lastDistance)}));
	}

	private void Cross(int dirKey, float offset) {
		Rigidbody rb = beer.GetComponent<Rigidbody> ();
		networkController.SendToServer (new Packet (gameController.version, C2M_Command.C2M_CROSS, new int[3]{dirKey, Mathf.RoundToInt(offset), this.GetDistance(lastDistance)}, new float[3]{rb.velocity.x, 0, rb.velocity.z}));
		Destroy (beer);
	}

	private void GameOver() {
		gameController.SwitchPhases (Phases.Replay);
	}
		
	private void RepopulateBeer(GameObject beer) {
		Vector3 scale = beer.transform.localScale;
		beer.transform.localScale = new Vector3 (scale.x * 10, scale.y * 10, scale.z * 10);
		beer.transform.position += new Vector3 (0, 0.5f, 0);
	}

	public void OnPressOk(bool ok) {
		Destroy (beer);
		if (ok) {
			gameController.SwitchPhases (Phases.LinkDevice);
		} else {
			SceneManager.LoadScene(0);
		}
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
		float z = packet.f_datas [2];
		Vector3 new_position = directionMap [dirKey] + (offsetMap [dirKey] * offset);
		beer = Instantiate (beer_prefab, new_position, Quaternion.Euler (new Vector3 (-90, 0, 0)), parent.transform);
		RepopulateBeer (beer);
		beer.GetComponent<Rigidbody> ().AddForce (new Vector3 (x * forceMultiplication, 0.0f, z * forceMultiplication));

		this.lastDistance += distance;
	}


	public void M2C_Score(Packet packet){
		this.lastDistance = packet.datas [1];
		GameOver ();
	}

	void Delete_all_objects(string[] tags){
		foreach (string tag in tags) {
			GameObject[] gmos = GameObject.FindGameObjectsWithTag (tag);
			foreach (GameObject g in gmos) Destroy (g);
		}
	}
}
