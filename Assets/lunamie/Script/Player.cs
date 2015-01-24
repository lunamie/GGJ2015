using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	[SerializeField]
	Camera camera;

	void Start() {
		var pView = this.GetComponent<PhotonView>();
		if( pView.isMine ) {
			GameManager.Instance.nextScene = "scene_2";
			GameManager.Instance.view = pView;
		}
		GameManager.Instance.playercnt++;
		//camera.enabled = pView.isMine;
		DontDestroyOnLoad( gameObject );

	}
	[RPC]
	void RecvStageClear() {
		//メッセージを一時的に遮断.
		GameManager.Instance.StageChange();

	}
}
