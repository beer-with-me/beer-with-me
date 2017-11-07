using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {
	
	public GamePlay_Manager gameplay_manager;
	private Rigidbody rb;
	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private bool firstPressed = false;
	private bool isMoving = false;
	private Vector3 initPos;
	private float force = 2.0f;

	void Start() {
		rb = this.gameObject.GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		if (gameplay_manager.isPlaying) {
			SwipeHandler ();
			float magnitude = rb.velocity.magnitude;
			if (magnitude > 0.5 && !isMoving) {
				isMoving = true;
			}
			if(isMoving && magnitude == 0.0f) {
				isMoving = false;
				gameplay_manager.lastDistance = Vector3.Distance (rb.position, initPos);
				gameplay_manager.Stop ();
			}
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
		initPos = rb.position;
		rb.AddForce (new Vector3 (currentSwipe.x * force, 0, currentSwipe.y * force));
	}
		
}
