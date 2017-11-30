using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour {
	
	public GamePlay_Manager gameplay_manager;
	public Rigidbody rb;
	private Vector2 firstPressPos;
	private float firstPressTime;
	private Vector2 secondPressPos;
	private float secondPressTime;
	private bool firstPressed = false;
	private bool isMoving = false;
	public  Vector3 initPos;

	void Start() {
		gameplay_manager = GameObject.Find ("GamePlayPhase").GetComponent<GamePlay_Manager> ();
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
				float distance = Vector3.Distance (rb.position, initPos);
				gameplay_manager.lastDistance += distance;
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
			firstPressTime = Time.time;
			firstPressed = true;
		}
		if(Input.GetMouseButtonUp(0) && firstPressed) {
			secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			MoveObject ();
			firstPressed = false;
		}
	}

	public void SwipeHandlerMobile() {
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began) {
				firstPressPos = new Vector2(t.position.x,t.position.y);
				firstPressTime = Time.time;
				firstPressed = true;
			}
			if(t.phase == TouchPhase.Ended && firstPressed) {
				secondPressPos = new Vector2(t.position.x,t.position.y);
				MoveObject ();
				firstPressed = false;
			}
		}
	}

	private void MoveObject () {
		Vector2 currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
		float deltaTime = Time.time - firstPressTime;
		initPos = rb.position;
		float force = Sigmoid (Mathf.Pow(1 / deltaTime, 2)) * gameplay_manager.forceMultiplication;
		rb.AddForce (new Vector3 (currentSwipe.x * force, 0, currentSwipe.y * force));
	}

	private float Sigmoid(float x) {
		return 2 / (1 + Mathf.Exp(-2 * x)) - 1;
	}
		
}
