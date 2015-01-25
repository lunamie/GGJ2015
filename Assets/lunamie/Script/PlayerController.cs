using UnityEngine;
using System.Collections;

public class PlayerController : Photon.MonoBehaviour {

	public enum PlayerState
	{
		Idle,
		Jump,
		Run,
	}

	public float jumpPower = 100.0f;
	public float movePower = 5.0f;

	public PlayerState	State { get; set; }	
	public float InputX { get; set; }
	//public float InputY { get: set; }

	void Start () {
	}
	
	void Update () {

		if( PlayerState.Jump == State )
		{
			return;
		}

		//自プレイヤーであるか評価
		if(photonView.isMine)
		{
			//ジャンプ
			if( Input.GetButtonDown("Jump") ) 
			{
				rigidbody.AddForce( Vector3.up * jumpPower );
				
				State = PlayerState.Jump;
			}
			else
			{
				//移動
				InputX = Input.GetAxis("Horizontal");
				//InputY = Input.GetAxis("Vertical");
				
				//Debug.Log("photonView: " + photonView.isMine + " player.ID: " + PhotonNetwork.player.ID);
				
				// 位置移動でのテスト
				//transform.position += new Vector3(InputX, 0.0f, 0.0f) * 0.1;
				//transform.position  = new Vector3(transform.position.x+InputX*0.1f, 0.0f, 0.0f);
				
				// Physicsで移動する。
				Vector3 force = new Vector3(movePower*InputX, 0.0f, 0.0f);
				rigidbody.AddForce(force);
				
				State = PlayerState.Run;
			}
		}
	}

	// 衝突コールバック
	void OnCollisionEnter(Collision col)
	{
		if( col.gameObject.CompareTag("Finish"))
		{
			if( null != GameManager.GetInstance() )
			{
				GameManager.GetInstance().StageClear();
			}
			State = PlayerState.Idle;
		}

		/*
		if( col.gameObject.CompareTag("Cube")) 
		{
		}
		*/

		if( PlayerState.Jump == State )
		{
			State = PlayerState.Idle;
		}
		else if( PlayerState.Run == State )
		{
			if( Mathf.Abs( InputX ) < 1.0f )
			{
				State = PlayerState.Idle;
			}
		}
	}
}
