using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum Phases{
	ConnectSetup,
	LinkDevice,
	GamePlay,
	Replay
}

public delegate void Dialog_Delegate(bool option);
public delegate void Subscriptor_Delegate(Packet packet);

public class Pair{
	public int first;
	public int second;
}

public class GameController : MonoBehaviour {
	public NetworkController networkController;

	public Transform dialog_folder;
	public GameObject Y_dialog_gmo;
	public Dialog_Delegate dialog_Delegate;
	public GameObject DialogCanvas;
	public bool has_dialog;

	public Text ping_text;
	public int version = 1;
	public Phases now_Phase;
	public bool phase_has_change;
	public GameObject connectSetup_gmo;
	public GameObject linkDevice_gmo;
	public GameObject gamePlay_gmo;
	public GameObject replay_gmo;
	public Camera mainCamera;
	public Camera replayCamera;

	public float editor_height_length;
	public float editor_width_length;
	public float editor_dpi;

	[HideInInspector] public float height_length;
	[HideInInspector] public float width_length;
	[HideInInspector] public float dpi;
	[HideInInspector] public bool is_Hoster;
	[HideInInspector] public int room_ID;

	[HideInInspector] public bool start_Here;
	[HideInInspector] public bool square_Here;
	[HideInInspector] public Vector2 start_Position;

	void Start(){
		has_dialog = false;
		Get_Device_Size ();
		is_Hoster = false;
		room_ID = -1;
		start_Here = false;
		square_Here = false;
		phase_has_change = false;
		SwitchPhases(Phases.ConnectSetup);
	}

	void Update(){
		if (phase_has_change) {
			phase_has_change = false;
			connectSetup_gmo.SetActive (now_Phase == Phases.ConnectSetup);
			linkDevice_gmo.SetActive (now_Phase == Phases.LinkDevice);
			gamePlay_gmo.SetActive (now_Phase == Phases.GamePlay);
			replay_gmo.SetActive (now_Phase == Phases.Replay);
			if (now_Phase == Phases.Replay) {
				mainCamera.gameObject.SetActive (false);
				replayCamera.gameObject.SetActive (true);
			} else {
				mainCamera.gameObject.SetActive (true);
				replayCamera.gameObject.SetActive (false);
			}
		}
	}

	// 取得裝置大小
	void Get_Device_Size(){
		dpi = Screen.dpi;
		if (dpi == 0) {
			dpi = 400.0f;
		}
		height_length = Screen.height / dpi * 4;
		width_length = Screen.width / dpi * 4;
		mainCamera.orthographicSize = width_length * height_length / 2;
	}

	public void SwitchPhases(Phases phase){
		now_Phase = phase;
		phase_has_change = true;
	}

	public void Start_Dialog(Dialog_Delegate d, string title, string content, int options_amount) { // dir=1 -> Y
//		if (has_dialog) return;
//		has_dialog = true;
//		GameObject dialog = Instantiate (Y_dialog_gmo, Vector2.zero, Quaternion.Euler(90, 0, 0),dialog_folder);
//		Vector3 scale = dialog.transform.localScale;
//		dialog.transform.localScale *= mainCamera.orthographicSize / 4;
//		dialog.GetComponent<Dialog_manager> ().dialog_Delegate = d;
//		dialog.GetComponent<Dialog_manager> ().options_amount = options_amount;
//		dialog.transform.Find ("canvas").Find ("Title").GetComponent<Text> ().text = title;
//		dialog.transform.Find ("canvas").Find ("Content").GetComponent<Text> ().text = content;
		DialogCanvas.SetActive(true);
		dialog_Delegate = d;
		DialogCanvas.transform.Find("Dialog").Find ("Title").GetComponent<Text> ().text = title;
		DialogCanvas.transform.Find("Dialog").Find ("Message").GetComponent<Text> ().text = content;
	}
}
