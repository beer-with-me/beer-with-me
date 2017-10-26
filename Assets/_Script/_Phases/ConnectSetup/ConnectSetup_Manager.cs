using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectSetup_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;


	public void Create_Room(){
		Debug.Log ("create");
		// 取得裝置大小

		// 向伺服端送出創建要求

		// 接收伺服端回傳的房間號碼

		// 將號碼顯示於螢幕上

		// 生成六方位按鈕
		Create_Link_Buttons();
	}


	public void Join_Room(){
		Debug.Log ("join");
		// 取得裝置大小

		// 讀取玩家輸入的房間號碼

		// 向伺服端送出加入要求

		// 生成六方位按鈕
		Create_Link_Buttons();
	}

	// 取得裝置大小
	Vector2 Get_Device_Size(){
		return Vector2.zero;
	}

	/* --------------- 串連地圖 --------------- */

	void Create_Link_Buttons(){
		// instantiate six link buttons

		// 顯示 「建立賽局」 按鈕
	}

	public void Press_Link_Buttons(){
		// 	若此邊尚未連接
		//		建立連接

		// 	若此邊已連接
		//		取消連接
	}

	public void Setup_Game(){
		// 向伺服器送出 建立賽局 要求

		// 向 GameController 回報階段任務完成 ， disable script
		gameController.SwitchPhases(Phases.GameSetup);
	}
}
