﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Start0 : MonoBehaviour {
	public LinkDevice_Manager linkDevice_Manager;

	void OnMouseDown(){
		StartCoroutine (linkDevice_Manager.Start0 ());
	}
}
