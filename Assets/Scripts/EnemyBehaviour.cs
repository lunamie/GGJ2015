using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float Speed;
	Vector2 direction;
	public float MaxSpeed;
	Vector3 startPosition;

	void Awake() {
		direction = Vector2.right;
		startPosition = transform.position;
	}

	// Update is called once per frame
	void Update () {

		// just move towards direction
		if (rigidbody2D.velocity.x*direction.x < MaxSpeed) {
			rigidbody2D.AddForce(direction*Speed);
		}

		// change direction if would fall down
		Vector2 position = new Vector2( transform.position.x, transform.position.y )+direction*5;
		Debug.DrawRay(transform.position, new Vector3(direction.x, 0, 0), Color.red, 0.1f);
		
		RaycastHit2D hit = Physics2D.Raycast( position, -Vector2.up, 3.0f, 1 << LayerMask.NameToLayer("Ground"));
		if (hit.collider == null) {
			direction = direction*-1;
		}
	}

	public void dealDamage() {
		gameObject.SetActive(false);
	}

	public void reset() {
		// reset position and velocity if there is still velocity left
		transform.position = startPosition;
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0;
		gameObject.SetActive(true);
	}
}
