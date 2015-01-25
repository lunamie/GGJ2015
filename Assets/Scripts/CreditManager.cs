using UnityEngine;
using System.Collections;

// Hong Linh Thai
// Script which processes the input

public class CreditManager : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey) {
			Application.LoadLevel("TitleScene");
		}
	}
}
