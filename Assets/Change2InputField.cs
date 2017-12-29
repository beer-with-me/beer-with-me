using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Change2InputField : MonoBehaviour {
	public GameObject inputField;

	public void Click(){
		inputField.SetActive (true);
		gameObject.SetActive (false);

		inputField.GetComponent<InputField>().Select ();
		Debug.Log ("highlighted");
	}
}
