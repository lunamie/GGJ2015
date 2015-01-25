using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {
	[SerializeField]
	int maxPlayer = 1;

	void Awake() {
		GameManager.Instance.sceneid = 2;
	}
	// Update is called once per frame
	void Update () {
		if( GameManager.Instance.playercnt >= maxPlayer ) {
			GameManager.Instance.StageClear( 2 );
			gameObject.SetActive( false );
		}
	}
}
