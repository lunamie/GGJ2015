using UnityEngine;
using System.Collections;

public class DoNotDestroyGameobject : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad( gameObject );
	}

}
