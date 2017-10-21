using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot_Ball : MonoBehaviour {
	public GameObject line_gmo; // 球的瞄準線
	public GameObject aim_circle_prefab; // 基準點 prefab
	public GameObject aim_circle; // 基準點
	public float speed_factor;
	public float min_radius;
	public float max_radius;
	Vector2 touch_position; // 手指點拉的初始點

	// Use this for initialization
	void Awake () {
		
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			//記錄初始點位
			touch_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Debug.Log (touch_position);

			// 生成基準點，並儲存
			aim_circle = Instantiate(aim_circle_prefab, touch_position, Quaternion.identity);


			// 顯示瞄準線
			line_gmo.SetActive (true);
		}

		if (Input.GetMouseButton (0)) {
			// 拉動
			Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 手指目前的點位
			Vector2 shooting_vector = touch_position - mouse_position;
			if (shooting_vector.x == 0)return;

			// 顯示線

			transform.rotation = Quaternion.Euler (new Vector3(0, 0, Mathf.Atan(shooting_vector.y / shooting_vector.x) * 180.0f / Mathf.PI + 90));

			// 生成基準點填充度
			float scale = shooting_vector.magnitude / max_radius;
			scale = Mathf.Clamp01 (scale);
			aim_circle.transform.Find("center").transform.localScale = new Vector3(scale, scale, scale);
		}


		if (Input.GetMouseButtonUp (0)) {
			Vector2 mouse_position = Camera.main.ScreenToWorldPoint(Input.mousePosition); // 手指目前的點位
			Vector2 velocity = touch_position - mouse_position;

			// 調整力道
			float radius = velocity.magnitude;
			velocity /= radius;
			radius = Mathf.Clamp (radius, min_radius, max_radius);
			velocity *= radius;
			velocity *= speed_factor;

			Debug.Log (velocity);
			GetComponent<Rigidbody2D> ().velocity = velocity;

			// 關閉瞄準線
			line_gmo.SetActive (false);

			// 刪除基準點
			Destroy(aim_circle);
		}
	}
}
