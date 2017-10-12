using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragItem : MonoBehaviour {


	void OnMouseDrag(){
		Vector3 pos =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos = new Vector3 (pos.x, pos.y, 0);

		// 調整位置 讓物件邊緣有吸附的效果

		transform.position = pos;
	}
}
