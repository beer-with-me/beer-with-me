using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Edge : MonoBehaviour {
	public int dirKey;
	public GamePlay_Manager gamePlay_manager;

	void OnCollisionEnter(Collision collisionInfo) {
		// Falling off the table / entering the other table
		if (collisionInfo.gameObject.name == "Beer") {
			gamePlay_manager.LeaveTable (dirKey);
		}
	}
}
