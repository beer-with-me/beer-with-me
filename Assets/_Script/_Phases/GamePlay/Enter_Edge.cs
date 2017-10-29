using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Edge : MonoBehaviour {
	public GamePlay_Manager gamePlay_manager;

	void OnCollisionExit(Collision collisionInfo) {
		// Falling off the table / entering the other table
		if (collisionInfo.gameObject.name == "Table") {
			gamePlay_manager.LeaveTable ();
		}
	}
}
