using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Hong Linh Thai
// Script which manages the level, e.g. reset the enemies 

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;
	public GameObject[] enemies;
	public GameObject player;
	public Vector3 startPosition;

	void Awake()  {
		instance = this;
		enemies = GameObject.FindGameObjectsWithTag("Enemy");
	}

	public void resetLevel() {
		foreach (GameObject obj in enemies) {
			obj.GetComponent<EnemyBehaviour>().reset();
		}
	}
	
	public void createNewLevelPart(Vector3 pos, float direction) {
		
	}
}
