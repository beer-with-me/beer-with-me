using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkDevice_Manager : MonoBehaviour {
	public GameController gameController;
	public NetworkController networkController;
	public Text room_ID_text;
	public Color[] colors;
	public GameObject[] link_Blocks;

	void OnEnable () {
		room_ID_text.text = gameController.room_ID.ToString();
	}

	public IEnumerator Press_Link_Buttons(int order){
		// 	若此邊尚未連接 -> 建立連接
		networkController.SendToServer(new Pocket(gameController.version, C2M_Command.C2M_LINK_KEY, new int[1]{order}));

		float connect_time = 0.0f;
		while (networkController.now_Pocket == null) {
			connect_time += Time.deltaTime;
			yield return null;
		}

		int dir = networkController.now_Pocket.datas [0];
		int color_index = networkController.now_Pocket.datas [1];
		networkController.now_Pocket = null;

		Change_Color (dir, colors[color_index]);

		// 	若此邊已連接 -> 取消連接
	}

	public void Change_Color(int dir, Color color){
		link_Blocks [dir - 1].GetComponent<SpriteRenderer> ().color = color;
	}

	public IEnumerator Start0(){
		// 建立 start0 賽局
		networkController.SendToServer(new Pocket(gameController.version, C2M_Command.C2M_START0, new int[0]{}));

		float connect_time = 0.0f;
		while (networkController.now_Pocket == null) {
			connect_time += Time.deltaTime;
			yield return null;
		}

		gameController.start_Here = (networkController.now_Pocket.datas [0] == 0) ? false : true;
		gameController.start_Position = new Vector2(networkController.now_Pocket.datas [1], networkController.now_Pocket.datas [2]);
		networkController.now_Pocket = null;


		// 向 GameController 回報階段任務完成 ， disable script
		gameController.SwitchPhases(Phases.GamePlay);
	}
}
