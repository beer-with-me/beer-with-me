using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Ball : MonoBehaviour {
	public GameObject line_gmo; // 球的瞄準線
	Vector2 touch_position; // 手指點拉的初始點

	// Use this for initialization
	void Awake () {
		
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			//記錄初始點位
			touch_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Debug.Log (touch_position);

			line_gmo.SetActive (true);
		}

		if (Input.GetMouseButton (0)) {
			// 拉動
			Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 手指目前的點位
			Vector2 shooting_vector = touch_position - mouse_position;
			if (shooting_vector.x == 0)return;

			Debug.Log (Mathf.Atan(shooting_vector.y / shooting_vector.x) * 180.0f / Mathf.PI + 90);

			// 顯示線

			transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.Atan(shooting_vector.y / shooting_vector.x) * 180.0f / Mathf.PI + 90));

			// 顯示圓圈
		}


		if (Input.GetMouseButtonUp (0)) {
			Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 手指目前的點位
			Vector2 velocity = touch_position - mouse_position;

			Debug.Log (velocity);
			GetComponent<Rigidbody2D> ().velocity = velocity;

			line_gmo.SetActive (false);
		}
	}
}
