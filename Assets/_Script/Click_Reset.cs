using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Click_Reset : MonoBehaviour {

	void OnMouseDown(){
		SceneManager.LoadScene(0);
	}
}
