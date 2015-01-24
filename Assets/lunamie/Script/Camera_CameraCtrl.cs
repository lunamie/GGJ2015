using UnityEngine;
using System.Collections;

public class Camera_CameraCtrl : Photon.MonoBehaviour {

	Vector3 oldMousePosition = new Vector3();
	public float speed = 0.2f;

	// Use this for initialization
	void Start () {
		oldMousePosition = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 newMousePosition = Input.mousePosition;
		Vector3 diff = (newMousePosition - oldMousePosition);
		diff.z = 0;
		float m = diff.magnitude * speed;
		diff.Normalize();
		diff.Scale(new Vector3(m,m,0));
		oldMousePosition = newMousePosition;

		this.transform.position += diff;
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//現在地と角度を送信
			stream.SendNext (this.transform.position);
		} else {
			this.transform.position = (Vector3)stream.ReceiveNext ();
		}
	}
}
