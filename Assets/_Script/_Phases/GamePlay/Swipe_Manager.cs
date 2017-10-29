﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe_Manager : MonoBehaviour {

	private Vector2 firstPressPos;
	private Vector2 secondPressPos;
	private Vector2 currentSwipe;
	private float force = 2.0f;

	// Update is called once per frame
	void Update () {
		Swipe ();
		SwipeMobile ();
	}

	public void Swipe()
	{
		if(Input.GetMouseButtonDown(0))
		{
			//save began touch 2d point
			firstPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
		}
		if(Input.GetMouseButtonUp(0))
		{
			secondPressPos = new Vector2(Input.mousePosition.x,Input.mousePosition.y);
			currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
			this.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (currentSwipe.x * force, 0, currentSwipe.y * force));
		}
	}

	public void SwipeMobile()
	{
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				//save began touch 2d point
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				secondPressPos = new Vector2(t.position.x,t.position.y);
				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);
				this.gameObject.GetComponent<Rigidbody> ().AddForce (new Vector3 (currentSwipe.x * force, 0, currentSwipe.y * force));
			}
		}
	}
}
