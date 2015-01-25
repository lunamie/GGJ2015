﻿using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	public string nextLevel;

	void OnCollisionStay2D(Collision2D collisionInfo){
		if(collisionInfo.collider.tag == "Player") {
			Debug.Log("start load level");
			if(PhotonNetwork.connected == true) {
				PhotonNetwork.automaticallySyncScene = true;
				PhotonNetwork.LoadLevel( nextLevel );
			} else {
				Application.LoadLevel(nextLevel);
			}
		}
	}
}
