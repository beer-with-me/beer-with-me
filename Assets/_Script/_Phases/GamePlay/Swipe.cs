using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {
	
	public GamePlay_Manager gameplay_manager;
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private bool firstPressed = false;
	private float force = 2.0f;

	void Update () {
		if (gameplay_manager.isPlaying) {
			SwipeHandler ();
		}
	}
		
	public void SwipeHandler() {
		if (SystemInfo.deviceType == DeviceType.Desktop) {
			SwipeHandlerDesktop ();
		} else {
			SwipeHandlerMobile ();
		}
	}

	public void SwipeHandlerDesktop() {
		if(Input.GetMouseButtonDown(0)) {
			firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			firstPressed = true;
		}
		if(Input.GetMouseButtonUp(0) && firstPressed) {
			secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			MoveObject (firstPressPos, secondPressPos);
			firstPressed = false;
		}
	}

	public void SwipeHandlerMobile() {
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began) {
				firstPressPos = new Vector2(t.position.x,t.position.y);
				firstPressed = true;
			}
			if(t.phase == TouchPhase.Ended && firstPressed) {
				secondPressPos = new Vector2(t.position.x,t.position.y);
				MoveObject (firstPressPos, secondPressPos);
				firstPressed = false;
			}
		}
	}

	private void MoveObject (Vector2 firstPressPos, Vector2 secondPressPos) {
		Vector2 currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
		this.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (currentSwipe.x * force, 0, currentSwipe.y * force));
	}
		
}
