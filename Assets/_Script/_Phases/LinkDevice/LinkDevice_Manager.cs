using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkDevice_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	public Text room_ID_text;

	void OnEnable () {
		room_ID_text.text = gameController.room_ID.ToString();
	}

	public void Press_Link_Buttons(){
		// 	若此邊尚未連接
		//		建立連接

		// 	若此邊已連接
		//		取消連接
	}
}
