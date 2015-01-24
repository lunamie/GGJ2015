using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectDisappearScript : MonoBehaviour {

	public GameObject[] toDisappear;
	public GameObject[] toAppear;

	void Update() {
		if(Vector3.Distance(transform.position, LevelManager.instance.player.transform.position) < 15.0f) {
			this.renderer.enabled = false;
		} else {
			this.renderer.enabled = true;
		}
	}
}
