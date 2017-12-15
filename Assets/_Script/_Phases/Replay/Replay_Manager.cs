using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay_Manager : MonoBehaviour {

	public GameController gameController;
	public GamePlay_Manager gamePlay_Manager;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnEnable () {
		Debug.Log ("Show Replay");

		Animator anim = GetComponent<Animator> ();
		// type: 1=success, 2=fail
		if (gamePlay_Manager.isSuccessful) {
			anim.SetInteger ("type", 1);
		} else {
			anim.SetInteger ("type", 2);
		}
		ShowDistance (gamePlay_Manager.lastDistance);
	}

	public void OnPressOk(bool ok) {
		if (ok) {
			gameController.SwitchPhases (Phases.LinkDevice);
		} else {
			SceneManager.LoadScene(0);
		}
	}

	void ShowDistance (float distance) {
		Debug.Log (distance);
		// gameController.Start_Dialog (OnPressOk, "Score", distance.ToString (), 2);
	}
}
