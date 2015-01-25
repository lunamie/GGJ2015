using UnityEngine;
using System.Collections;

// Hong Linh Thai
// Script which sets the camera to follow UnityChan

public class CameraFollowScript : MonoBehaviour {

	public float distance;
	public static CameraFollowScript instance;

	public void Awake() {
		instance = this;
	}

	public void Update() {
		transform.position = new Vector3(LevelManager.instance.player.transform.position.x, LevelManager.instance.player.transform.position.y+1, -distance);
	}


}
