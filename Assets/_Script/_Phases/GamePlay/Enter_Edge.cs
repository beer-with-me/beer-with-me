using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter_Edge : MonoBehaviour {
	public int dirKey;
	public GamePlay_Manager gamePlay_manager;

	void OnCollisionEnter(Collision collisionInfo) {
		// Falling off the table / entering the other table
		if (collisionInfo.gameObject.tag == "Beer") {
			// First leave
			if (gamePlay_manager.swipe) {
				float distance = Vector3.Distance (gamePlay_manager.swipe.rb.position, gamePlay_manager.swipe.initPos);
				gamePlay_manager.lastDistance += distance;
				gamePlay_manager.isLeavingTable = true;
				gamePlay_manager.EnterEdge (dirKey);
			} else {
				// Non-first enter
				gamePlay_manager.swipe = collisionInfo.gameObject.GetComponent<Swipe>();
			}
		}
	}
}
