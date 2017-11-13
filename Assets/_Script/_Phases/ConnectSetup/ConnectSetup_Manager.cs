using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class ConnectSetup_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	public KeyBoard_Handler keyBoard_Handler;
	private int serverReceiveIndex;

	private bool onReceive;
	private Packet receivePacket;

	void OnEnable(){
		onReceive = false;
		serverReceiveIndex = networkController.AddSubscriptor (new Subscriptor(OnReceive, new M2C_Command[2]{M2C_Command.M2C_CREATE, M2C_Command.M2C_JOIN}));
	}
	void OnDisable () {
		networkController.RemoveSubscriptor (serverReceiveIndex);
	}

	void Update(){
		if (onReceive) {
			AnalysisReceive (receivePacket);
			onReceive = false;
		}
	}



	// -------------  start sending data ------------- //



	public void Create_Room(){
		// 取得裝置大小
		Vector2 device_size = Get_Device_Size();

		// 向伺服端送出創建要求
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_CREATE, new int[2]{(int)(device_size.x * 100), (int)(device_size.y * 100)}));
	}


	public void Join_Room(){
		// 取得裝置大小
		Vector2 device_size = Get_Device_Size();

		// 向伺服端送出加入要求
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_JOIN, new int[3]{keyBoard_Handler.room_ID, (int)(device_size.x * 100), (int)(device_size.y * 100)}));

	}


	// 取得裝置大小
	Vector2 Get_Device_Size(){
		#if UNITY_EDITOR
			return new Vector2(2.49f, 4.42f);
		#else
			AndroidJavaClass activityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject activity = activityClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject metrics = new AndroidJavaObject("android.util.DisplayMetrics");
			activity.Call<AndroidJavaObject>("getWindowManager").Call<AndroidJavaObject>("getDefaultDisplay").Call("getMetrics", metrics);
			float dpi = (metrics.Get<float>("xdpi") + metrics.Get<float>("ydpi")) * 0.5f;
			return new Vector2(Screen.height/dpi, Screen.width/dpi);
		#endif
	}



	// ------------- end of sending data ------------- //
	// -------------  start receive data ------------- //



	public void OnReceive(Packet packet) {
		onReceive = true;
		receivePacket = packet;
	}

	public void AnalysisReceive(Packet packet){
		Debug.Log ("ConnectSetup receive");
		switch (packet.M2C_command) {
		case M2C_Command.M2C_CREATE:
			M2C_Create (packet);
			break;
		case M2C_Command.M2C_JOIN:
			M2C_Join (packet);
			break;
		default:
			break;
		}
	}

	public void M2C_Join(Packet packet){
		if (packet.datas [0] == 0) {
			// 加入成功
			gameController.room_ID = keyBoard_Handler.room_ID;
			Setup_Game();
		} else {
			gameController.Start_Dialog (null, "Error", "Can't find this room.", 1);
		}
	}

	public void M2C_Create(Packet packet){
		// 接收伺服端回傳的房間號碼
		gameController.room_ID = packet.datas [0];

		// 創建成功
		Setup_Game ();
	}



	// ------------- end of receive data ------------- //



	public void Setup_Game(){
		// 向 GameController 回報階段任務完成 ， disable script
		gameController.SwitchPhases(Phases.LinkDevice);
	}
}
