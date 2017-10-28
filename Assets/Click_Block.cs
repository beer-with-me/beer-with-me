using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Block : MonoBehaviour {
	public KeyBoard_Handler keyBoard_Handler;
	public int key;

	void OnMouseDown(){
		keyBoard_Handler.Key_In (key);
	}
}
