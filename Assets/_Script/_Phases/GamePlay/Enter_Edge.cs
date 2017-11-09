using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Edge : MonoBehaviour {
	public int dirKey;
	public GamePlay_Manager gamePlay_manager;

	void OnCollisionEnter(Collision collisionInfo) {
		// Falling off the table / entering the other table
		if (collisionInfo.gameObject.tag == "Beer") {
			gamePlay_manager.lastDistance = Vector3.Distance (gamePlay_manager.swipe.rb.position, gamePlay_manager.swipe.initPos);
			gamePlay_manager.isLeavingTable = true;
			gamePlay_manager.EnterEdge (dirKey);
		}
	}
}
