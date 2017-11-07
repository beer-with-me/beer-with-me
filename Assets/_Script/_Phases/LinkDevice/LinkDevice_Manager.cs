using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkDevice_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	public Text room_ID_text;
	public Color init_Color;
	public Color clicked_Color;
	public Color[] colors;
	public GameObject[] link_Blocks;

	void OnEnable () {
		room_ID_text.text = gameController.room_ID.ToString();
	}

	public void Press_Link_Buttons(int order){
		// 對按下的按鈕塗色
		Draw_Clicked_Color(order);

		// 	若此邊尚未連接 -> 建立連接
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_LINK_KEY, new int[1]{order}));
		networkController.ReceiveFromServer (RFS_Press_Link_Buttons);

		// 	若此邊已連接 -> 取消連接
	}

	public void RFS_Press_Link_Buttons(Packet packet){
		int dir = packet.datas [0];
		int color_index = packet.datas [1];
		Change_Color (dir, colors[color_index]);
	}

	void Draw_Clicked_Color(int order){
		for (int i = 0; i < 6; i++) {
			if (link_Blocks [i].GetComponent<SpriteRenderer> ().color == clicked_Color) link_Blocks [i].GetComponent<SpriteRenderer> ().color = init_Color;
		}

		link_Blocks [order - 1].GetComponent<SpriteRenderer> ().color = clicked_Color;
	}

	public void Change_Color(int dir, Color color){
		link_Blocks [dir - 1].GetComponent<SpriteRenderer> ().color = color;
	}

	public void Start0(){
		// 建立 start0 賽局
		networkController.SendToServer(new Packet(gameController.version, C2M_Command.C2M_START0, new int[0]{}));
		networkController.ReceiveFromServer (RFS_Start0);
	}

	public void RFS_Start0(Packet packet){
		gameController.start_Here = (packet.datas [0] == 0) ? false : true;
		gameController.start_Position = new Vector2(packet.datas [1], packet.datas [2]);

		// 向 GameController 回報階段任務完成 ， disable script
		gameController.SwitchPhases(Phases.GamePlay);
	}
}
