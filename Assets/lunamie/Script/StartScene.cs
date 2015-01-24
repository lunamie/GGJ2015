using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		if( GameManager.Instance.playercnt > 1 ) {
			GameManager.Instance.StageClear( 1 );
			gameObject.SetActive( false );
		}
	}
}
