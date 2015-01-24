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

			//transform.position +=  new Vector3(inputX, 0.0f, 0.0f) * 0.1;
			transform.position = new Vector3(transform.position.x+inputX*0.1f, 0.0f, 0.0f);

			//Vector3 force = new Vector3(inputX, inputY, 0.0f) * movePower;
			//rigidbody.AddForce(force);
			
			//ジャンプ
			if(Input.GetButtonDown("Jump")) {
				rigidbody.AddForce(Vector3.up * jumpPower);
			}
		}
	}
}
