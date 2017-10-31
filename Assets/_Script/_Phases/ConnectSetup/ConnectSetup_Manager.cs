using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSetup_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	public KeyBoard_Handler keyBoard_Handler;

	public IEnumerator Create_Room(){
		// 取得裝置大小

		// 向伺服端送出創建要求
		networkController.SendToServer(new Pocket(gameController.version, C2M_Command.C2M_CREATE, new int[2]{1, 1}));

		// 接收伺服端回傳的房間號碼
		float connect_time = 0.0f;
		while (networkController.now_Pocket == null && Time.deltaTime <= 3.0f) {
			connect_time += Time.deltaTime;
			yield return null;
		}

		gameController.room_ID = networkController.now_Pocket.datas [0];
		networkController.now_Pocket = null;

		// 創建成功
		Setup_Game();
	}


	public IEnumerator Join_Room(){
		// 取得裝置大小

		// 讀取玩家輸入的房間號碼

		// 向伺服端送出加入要求
		networkController.SendToServer(new Pocket(gameController.version, C2M_Command.C2M_JOIN, new int[3]{keyBoard_Handler.room_ID, 1, 1}));

		float connect_time = 0.0f;
		while (networkController.now_Pocket == null && Time.deltaTime <= 3.0f) {
			connect_time += Time.deltaTime;
			yield return null;
		}

		if (networkController.now_Pocket.datas [0] == 0) {
			gameController.room_ID = keyBoard_Handler.room_ID;

			// 加入成功
			Setup_Game ();
		} else {
			gameController.Start_Dialog (Empty_Delegate, "Error", "Can't find this room.", 1);
		}
		networkController.now_Pocket = null;

	}

	public void Empty_Delegate(bool option){
	}

	// 取得裝置大小
	Vector2 Get_Device_Size(){
		return Vector2.zero;
	}

	public void Setup_Game(){
		// 向伺服器送出 串連地圖 要求

		// 向 GameController 回報階段任務完成 ， disable script
		gameController.SwitchPhases(Phases.LinkDevice);
	}
}
