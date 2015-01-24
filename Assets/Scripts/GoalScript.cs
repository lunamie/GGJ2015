using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour {

	public int nextLevel;

	void OnCollisionStay2D(Collision2D collisionInfo){
		if(collisionInfo.collider.tag == "Player") {
			Application.LoadLevel(nextLevel);
		}
	}
}
