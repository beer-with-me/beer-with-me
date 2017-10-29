using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Click_Link_Block : MonoBehaviour {
	public LinkDevice_Manager linkDevice_Manager;
	public int order;

	void OnMouseUp(){
		StartCoroutine(linkDevice_Manager.Press_Link_Buttons(order));
	}
}
