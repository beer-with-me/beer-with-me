﻿using System.Collections;
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

		// 向伺服端送出創建要求
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_CREATE, new int[2]{1, 1}));
	}


	public void Join_Room(){
		// 取得裝置大小

		// 讀取玩家輸入的房間號碼

		// 向伺服端送出加入要求
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_JOIN, new int[3]{keyBoard_Handler.room_ID, 1, 1}));

	}


	// 取得裝置大小
	Vector2 Get_Device_Size(){
		return Vector2.zero;
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
