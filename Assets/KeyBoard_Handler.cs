using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyBoard_Handler : MonoBehaviour {
	public int room_ID;
	public Text room_ID_text;

	void Start(){
		room_ID = 0;
	}

	public void Key_In(int x){
		if (x == -1) room_ID = 0;
		else if (room_ID <= 9999) {
			room_ID *= 10;
			room_ID += x;
		}
			
		room_ID_text.text = (room_ID == 0) ? "" : room_ID.ToString ();
	}
}
