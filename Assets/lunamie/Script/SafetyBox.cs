using UnityEngine;
using System.Collections;

public class SafetyBox : MonoBehaviour {

	static private SafetyBox instance = null;
	static public SafetyBox Instance {
		get {
			return GetInstance();
		}
	}
	static public SafetyBox GetInstance() {
		if( instance == null ) {
			GameObject obj = new GameObject( "SafetyBox" );
			instance = obj.AddComponent<SafetyBox>();

		}
		return instance;
	}

	void Awake() {
		if( instance != null ) {
			Destroy( this.gameObject );
			return;
		}
		instance = this;
	}

	void Start() {
		DontDestroyOnLoad( gameObject );
	}

	void AllRelease() {
		for( int i = 0; i < transform.childCount; i++ ) {
			Destroy( transform.GetChild( i ) );
		}
	}
}
