using UnityEngine;
using System.Collections;

public class CreditManager : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if(Input.anyKey) {
			Application.LoadLevel("TitleScene");
		}
	}
}
