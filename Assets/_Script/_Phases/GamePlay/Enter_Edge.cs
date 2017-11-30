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

				float oX = this.transform.position.x;
				float oZ = this.transform.position.z;
				float pX = collisionInfo.contacts [0].point.x;
				float pZ = collisionInfo.contacts [0].point.z;
				float offset = 0.0f;

				if (dirKey == 3 || dirKey == 6) {
					offset = pX - oX;
				} else {
					offset = pZ - oZ;
				}
				gamePlay_manager.isLeavingTable = true;
				gamePlay_manager.EnterEdge (dirKey, offset);
			} else {
				// Non-first enter
				gamePlay_manager.swipe = collisionInfo.gameObject.GetComponent<Swipe>();
			}
		}
	}
}
