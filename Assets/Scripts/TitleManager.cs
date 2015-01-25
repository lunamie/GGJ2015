using UnityEngine;
using System.Collections;

public class TitleManager : MonoBehaviour {

	void Awake() {
		FadeManager.Instance.Alpha = 1f;
		FadeManager.Instance.FadeIn( 0.5f, 0 );
	}
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space)) {
			Application.LoadLevel("StartScene");
		}
		if(Input.GetKeyDown(KeyCode.C)){
			Application.LoadLevel("Credits");
		}
	}
}
