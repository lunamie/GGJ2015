using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {
	[SerializeField]
	string sceneName;
	
	// Update is called once per frame
	void Update () {
		if( GameManager.Instance.playercnt > 1 ) {
			GameManager.Instance.StageClear( sceneName );
			gameObject.SetActive( false );
		}
	}
}
