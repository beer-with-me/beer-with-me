using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Create_Room_Button : MonoBehaviour {
	public ConnectSetup_Manager connectSetup_Manager;

	public void OnMouseUp(){
		connectSetup_Manager.Create_Room();
	}
}
