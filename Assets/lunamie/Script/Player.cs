using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	void Start() {
		var pView = this.GetComponent<PhotonView>();
		if( pView.isMine ) {
			GameManager.Instance.view = pView;
		}
		GameManager.Instance.playercnt++;
		DontDestroyOnLoad( gameObject );
	}

	[RPC]
	void RecvStageClear() {
		//メッセージを一時的に遮断.
		GameManager.Instance.StageChange();

	}
}
