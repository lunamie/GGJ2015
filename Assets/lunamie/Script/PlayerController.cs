using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour {
	
	public float jumpPower = 300f;
	public float movePower = 10f;
	
	void Start () {
	}
	
	void Update () {
		//自プレイヤーであるか評価
		if(photonView.isMine)
		{ 
			//移動
			float inputX = Input.GetAxis("Horizontal");
			float inputY = Input.GetAxis("Vertical");

			//Debug.Log("photonView: " + photonView.isMine + " player.ID: " + PhotonNetwork.player.ID);

			// 位置移動でのテスト
			//transform.position +=  new Vector3(inputX, 0.0f, 0.0f) * 0.1;
			//transform.position = new Vector3(transform.position.x+inputX*0.1f, 0.0f, 0.0f);

			// Physicsで移動する。
			Vector3 force = new Vector3(movePower*inputX, 0.0f, 0.0f);
			rigidbody.AddForce(force);
			
			//ジャンプ
			//if(Input.GetButtonDown("Jump")) {
			//	rigidbody.AddForce(Vector3.up * jumpPower);
			//}
		}
	}
}
