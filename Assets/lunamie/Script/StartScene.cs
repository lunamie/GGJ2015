﻿using UnityEngine;
using System.Collections;

public class StartScene : MonoBehaviour {
	[SerializeField]
	int maxPlayer = 1;

	[SerializeField]
	UILabel label;

	void Awake() {
		GameManager.Instance.sceneid = 2;
		GameManager.Instance.ConnectLog = "サーバーに接続中...";
	}
	// Update is called once per frame
	void Update () {
		label.text = GameManager.Instance.ConnectLog;
		
		if( GameManager.Instance.playercnt >= maxPlayer ) {
			GameManager.Instance.StageClear( 2 );
			gameObject.SetActive( false );
		}
	}
}
