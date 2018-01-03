using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Replay_Manager : MonoBehaviour {

	public GameController gameController;
	public GamePlay_Manager gamePlay_Manager;
	public AudioSource audioSuccess;
	public AudioSource audioFail;

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
		if (gamePlay_Manager.lastDistance != 0) {
			anim.SetInteger ("type", 1);
			ShowDistance ();
			audioSuccess.PlayOneShot (audioSuccess.clip);
		} else {
			anim.SetInteger ("type", 2);
			ShowDistance ();
			audioFail.PlayOneShot (audioFail.clip);
		}
	}

	public void OnPressOk(bool ok) {
		if (ok) {
			gameController.SwitchPhases (Phases.LinkDevice);
		} else {
			SceneManager.LoadScene(0);
		}
	}

	void ShowDistance () {
		gameController.Start_Dialog (OnPressOk, "再玩一次?", gamePlay_Manager.lastDistance.ToString () + " / " + gamePlay_Manager.highestScore.ToString (), 2);
	}
}
