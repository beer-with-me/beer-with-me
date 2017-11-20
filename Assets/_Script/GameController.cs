using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public enum Phases{
	ConnectSetup,
	LinkDevice,
	GameSetup,
	GamePlay,
	GameSettle
}

public delegate void Dialog_Delegate(bool option);
public delegate void Subscriptor_Delegate(Packet packet);

public class Pair{
	public int first;
	public int second;
}

public class GameController : MonoBehaviour {
	public NetworkController networkController;

	public GameObject dialog_gmo;
	public GameObject Y_dialog_gmo;
	public Dialog_Delegate dialog_Delegate;
	public bool has_dialog;

	public int version = 1;
	public Phases now_Phase;
	public GameObject connectSetup_gmo;
	public GameObject linkDevice_gmo;
	public GameObject gameSetup_gmo;
	public GameObject gamePlay_gmo;
	public GameObject gameSettle_gmo;

	public Camera MainCamera;
	public Camera PlayCamera;

	public float editor_height_length;
	public float editor_width_length;
	public float editor_dpi;

	[HideInInspector] public float height_length;
	[HideInInspector] public float width_length;
	[HideInInspector] public float dpi;
	[HideInInspector] public bool is_Hoster;
	[HideInInspector] public int room_ID;

	[HideInInspector] public bool start_Here;
	[HideInInspector] public Vector2 start_Position;

	void Start(){
		has_dialog = false;
		Get_Device_Size ();
		is_Hoster = false;
		room_ID = -1;
		start_Here = false;
		SwitchPhases(Phases.ConnectSetup);
	}

	// 取得裝置大小
	void Get_Device_Size(){
		#if UNITY_EDITOR
		height_length = editor_height_length;
		width_length = editor_width_length;
		dpi = editor_dpi;
		#else
		AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
		AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics");
		activity.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay").Call("getMetrics", metrics);
		dpi = (metrics.Get<float>("xdpi") + metrics.Get<float>("ydpi")) * 0.5f;
		height_length = Screen.height_DPI/dpi;
		width_length = Screen.width/dpi;
		#endif
	}

	public void SwitchPhases(Phases phase){
		now_Phase = phase;
		connectSetup_gmo.SetActive (false);
		linkDevice_gmo.SetActive (false);
		gameSetup_gmo.SetActive (false);
		gamePlay_gmo.SetActive (false);
		gameSettle_gmo.SetActive (false);
		if(phase == Phases.ConnectSetup) 	connectSetup_gmo.SetActive (true);
		if(phase == Phases.LinkDevice) 	linkDevice_gmo.SetActive (true);
		if(phase == Phases.GameSetup) 	gameSetup_gmo.SetActive (true);
		if(phase == Phases.GamePlay)		gamePlay_gmo.SetActive (true);
		if(phase == Phases.GameSettle)	gameSettle_gmo.SetActive (true);
		PlayCamera.enabled = phase == Phases.GamePlay;
		MainCamera.enabled = phase != Phases.GamePlay;
	}

	public void Start_Dialog(Dialog_Delegate d, string title, string content, int options_amount, int dir){ // dir=1 -> Y
		if (has_dialog) return;
		has_dialog = true;
		GameObject dialog = Instantiate ((dir == 0) ? dialog_gmo : Y_dialog_gmo, Vector2.zero, Quaternion.Euler(90 * dir, 0, 0));
		dialog.GetComponent<Dialog_manager> ().dialog_Delegate = d;
		dialog.GetComponent<Dialog_manager> ().options_amount = options_amount;
		dialog.transform.Find ("canvas").Find ("Title").GetComponent<Text> ().text = title;
		dialog.transform.Find ("canvas").Find ("Content").GetComponent<Text> ().text = content;
	}
}
