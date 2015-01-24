using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {
	[SerializeField]
	int maxPlayer = 1;
	// Update is called once per frame
	void Update () {
		if( GameManager.Instance.playercnt >= maxPlayer ) {
			GameManager.Instance.StageClear( 1 );
			gameObject.SetActive( false );
		}
	}
}
