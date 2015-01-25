/**
 * フェードマネージャクラス
 * Author Y.Numakura
 **/

using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// シーン遷移時のフェードイン・アウトを制御するためのクラス
/// </summary>
public class FadeManager : MonoBehaviour {
	/// <summary>暗転用黒テクスチャ</summary>
	private Texture2D blackTexture;
	/// <summary>フェード中の透明度</summary>
	public float Alpha {
		get;
		set;
	}
	/// <summary>フェード中かどうか</summary>
	private bool isFading = false;

	public Color color {
		get;
		set;
	}

	private IEnumerator coroutine;

	static private FadeManager instance = null;
	public static FadeManager Instance {
		get {
			if( !instance ) CreateInstance();
			return instance;
		}
	}

	private static void CreateInstance() {
		Debug.Log( "InstanceCreate" );
		GameObject obj = new GameObject();
		instance = obj.AddComponent<FadeManager>();
	}


	public void Awake() {
		if( instance ) {
			Destroy( this.gameObject );
			return;
		}
		instance = this;

		DontDestroyOnLoad( this.gameObject );

		//ここで黒テクスチャ作る
		this.blackTexture = new Texture2D( 32, 32, TextureFormat.RGB24, false );
		this.blackTexture.ReadPixels( new Rect( 0, 0, 32, 32 ), 0, 0, false );
		this.blackTexture.SetPixel( 0, 0, Color.white );
		this.blackTexture.Apply();
	}

	public void OnGUI() {

		//透明度を更新して黒テクスチャを描画
		GUI.color = new Color( this.color.r, this.color.g, this.color.b, this.Alpha );
		GUI.DrawTexture( new Rect( 0, 0, Screen.width, Screen.height ), this.blackTexture );
	}

	public void FadeOut( float _time, float _delay, Action _callback = null ) {
		if( this.coroutine != null ) return;
		this.coroutine = this.Fade( 1f, _time, _delay, _callback );
		StartCoroutine( this.coroutine );
	}

	public void FadeIn( float _time, float _delay, Action _callback = null ) {
		if( this.coroutine != null ) return;
		this.coroutine = this.Fade( -1f, _time, _delay, _callback );
		StartCoroutine( this.coroutine );
	}

	private IEnumerator Fade( float _direction, float _time, float _delay, Action _callback = null ) {
		if( _delay > 0f ) {
			yield return new WaitForSeconds( _delay );
		}
		while( this.Alpha <= 1f && this.Alpha >= 0f ) {
			this.Alpha += _direction * ( Time.deltaTime / _time );

			yield return 0;
		}

		this.Alpha = Mathf.Clamp01( this.Alpha );
		this.coroutine = null;
		if( _callback != null ) _callback();
		this.isFading = false;
		yield return true;
	}
}