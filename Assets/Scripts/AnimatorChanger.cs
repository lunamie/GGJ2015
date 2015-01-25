using UnityEngine;
using System.Collections;

public class AnimatorChanger : MonoBehaviour {
	Animator animator;
	[SerializeField]
	Rigidbody rid;

	enum State {
		idle,
		run,
		walk
	}
	State state = State.idle;
	float prev_x;
	// Update is called once per frame
	void Start() {
		if( !animator ) {
			animator = GetComponentInChildren<Animator>();
		}
		StartCoroutine( Move() );
	}

	IEnumerator Move() {
		while(true){
			var fix = ( transform.position.x - prev_x );
			Debug.Log( fix );
			var prev = state;

			float accel_run = 0.7f;
			float accel_walk = 0.1f;

			float accel = Mathf.Abs( fix );

			if( accel > accel_run ) {
				state = State.run;
			} else if( accel > accel_walk ) {
				state = State.walk;
			} else {
				state = State.idle;
			}

			if( fix > accel_walk ) {
				animator.transform.rotation = Quaternion.AngleAxis( 90, Vector3.up );
			} else if( fix < -accel_walk ) {
				animator.transform.rotation = Quaternion.AngleAxis( 90, Vector3.down );
			}
			if( prev != state ) {
				animator.SetTrigger( state.ToString() );
			}
			prev_x = transform.position.x;
			yield return new WaitForSeconds( 0.5f );
		}
	}
}
