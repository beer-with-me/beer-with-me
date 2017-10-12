using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Add_buttom : MonoBehaviour {
	private GameController gameController;

	// Use this for initialization
	void Start () {
		gameController = GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	void OnMouseDown(){
		Instantiate (gameController.square, Vector3.zero, Quaternion.identity, gameController.mapBlocksFolder);

		//要創在現有的地圖的臨邊上
	}
}
