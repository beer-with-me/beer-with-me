using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Join_Room_Button : MonoBehaviour {
	public ConnectSetup_Manager connectSetup_Manager;

	void OnMouseUp(){
		connectSetup_Manager.Join_Room();
	}
}
