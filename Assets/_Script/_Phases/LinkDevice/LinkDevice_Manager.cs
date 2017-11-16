using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LinkDevice_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	private int serverReceiveIndex;

	private bool onReceive;
	private Packet receivePacket;

	public Text room_ID_text;
	public Color init_Color;
	public Color clicked_Color;
	public Color[] colors;
	public GameObject[] link_Blocks;

	void OnEnable () {
		room_ID_text.text = gameController.room_ID.ToString();
		serverReceiveIndex = networkController.AddSubscriptor (new Subscriptor(OnReceive, new M2C_Command[2]{M2C_Command.M2C_LINK, M2C_Command.M2C_WAIT_FIRE}));
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



	public void Press_Link_Buttons(int order){
		// 對按下的按鈕塗色
		Draw_Clicked_Color(order);

		// 	若此邊尚未連接 -> 建立連接
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_LINK_KEY, new int[1]{order}));

		// 	若此邊已連接 -> 取消連接
	}

	public void Start0(){
		// 建立 start0 賽局
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_START0, new int[0]{}));
	}



	// ------------- end of sending data ------------- //
	// -------------  start receive data ------------- //



	public void OnReceive(Packet packet) {
		onReceive = true;
		receivePacket = packet;
	}

	public void AnalysisReceive(Packet packet){
		Debug.Log ("LinkDevice receive");
		switch (packet.M2C_command) {
		case M2C_Command.M2C_LINK:
			M2C_Link (packet);
			break;
		case M2C_Command.M2C_WAIT_FIRE:
			M2C_Wait_Fire (packet);
			break;
		default:
			break;
		}
	}
		
	public void M2C_Link(Packet packet){
		int dir = packet.datas [0];
		int color_index = packet.datas [1];
		Change_Color (dir, colors[color_index]);
	}

	public void M2C_Wait_Fire(Packet packet){
		gameController.start_Here = (packet.datas [0] == 0) ? false : true;
//		gameController.start_Position = new Vector2(packet.datas [1], packet.datas [2]);
		gameController.start_Position = new Vector2(0, 0);

		// 向 GameController 回報階段任務完成 ， disable script
		gameController.SwitchPhases(Phases.GamePlay);
	}



	// ------------- end of receive data ------------- //



	public void Change_Color(int dir, Color color){
		link_Blocks [dir - 1].GetComponent<SpriteRenderer> ().color = color;
	}

	void Draw_Clicked_Color(int order){
		for (int i = 0; i < 6; i++) {
			if (link_Blocks [i].GetComponent<SpriteRenderer> ().color == clicked_Color) link_Blocks [i].GetComponent<SpriteRenderer> ().color = init_Color;
		}

		link_Blocks [order - 1].GetComponent<SpriteRenderer> ().color = clicked_Color;
	}
}
