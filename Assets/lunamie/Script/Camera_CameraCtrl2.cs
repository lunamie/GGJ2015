using UnityEngine;
using System.Collections;

public class Camera_CameraCtrl2 : Photon.MonoBehaviour {

	public GameObject target = null;

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if (target) {
			transform.position = new Vector3( target.transform.position.x, target.transform.position.y, transform.position.z);
		}
	}
}
