using UnityEngine;
using System.Collections;

public class Synchronizer : Photon.MonoBehaviour {

	private Vector3		receivePosition = Vector3.zero;
	private Quaternion	receiveRotation = Quaternion.identity;
	private Vector3		receiveVelocity = Vector3.zero;

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{
		if( stream.isWriting )
		{
			//データの送信
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
			//stream.SendNext(rigidbody2D.velocity);
		}
		else
		{
			//データの受信
			receivePosition = (Vector3)stream.ReceiveNext();
			receiveRotation = (Quaternion)stream.ReceiveNext();
			//receiveVelocity = (Vector2)stream.ReceiveNext();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( !photonView.isMine )
		{
			transform.position   = Vector3.Lerp(transform.position, receivePosition, Time.deltaTime * 10);
			transform.rotation   = Quaternion.Lerp(transform.rotation, receiveRotation, Time.deltaTime * 10);
			//rigidbody2D.velocity = Vector2.Lerp(rigidbody2D.velocity, receiveVelocity, Time.deltaTime * 10);
		}
	}

}
