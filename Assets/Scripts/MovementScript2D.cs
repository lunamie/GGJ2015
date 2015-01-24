using UnityEngine;
using System.Collections;

public class MovementScript2D : MonoBehaviour {

	public float Speed;
	public float JumpForce;
	public float MaxSpeed;
	Vector3 startPosition;
	public float falling_treshhold;

	public GameObject playerPrefab;

	Rigidbody2D m_Body;
	bool m_IsGrounded;
	
	void Awake() 
	{
		m_Body = GetComponent<Rigidbody2D>();
		startPosition = transform.position;
	}

	void Update() 
	{
		UpdateIsGrounded();
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		Vector2 movementVelocity = m_Body.velocity;
		
		if( Input.GetAxisRaw( "Horizontal" ) > 0.5f )
		{
			if (rigidbody2D.velocity.x < MaxSpeed) {
				rigidbody2D.AddForce(new Vector2(Speed, 0.0f));
			}
			
		}
		else if( Input.GetAxisRaw( "Horizontal" ) < -0.5f )
		{
			if (rigidbody2D.velocity.x > -MaxSpeed) {
				rigidbody2D.AddForce(new Vector2(-Speed, 0.0f));
			}
		}
		else
		{
			movementVelocity.x = 0;
		}
		
		m_Body.velocity = movementVelocity;

		if( Input.GetKeyDown( KeyCode.Space ) == true && m_IsGrounded == true )
		{
			m_Body.AddForce( Vector2.up * JumpForce );
		}

		if(transform.position.y < falling_treshhold) {
			reset ();
			
			// reset the enemies
			LevelManager.instance.resetLevel();
		}

	}

	void UpdateIsGrounded()
	{
		Vector2 position = new Vector2( transform.position.x, transform.position.y );

		RaycastHit2D hit = Physics2D.Raycast( position, -Vector2.up, 3.0f, 1 << LayerMask.NameToLayer("Ground"));
		m_IsGrounded = hit.collider != null;

	}

	void OnCollisionEnter2D(Collision2D other){
		// Kill the player if he hits an enemy
		if (other.collider.tag.Equals("Enemy")) {
			reset ();

			// reset the enemies
			LevelManager.instance.resetLevel();
		}
		// Kill the enemy if the player hits the enemy damage part
		if (other.collider.tag.Equals("EnemyDamage")) {
			Debug.Log(other.gameObject.transform.name);
			other.gameObject.transform.GetComponent<EnemyBehaviour>().dealDamage();
		}


	}

	void OnTriggerEnter2D(Collider2D other) {
		// if it is a checkpoint create new level part
		if (other.tag.Equals("CheckpointRight") && (rigidbody2D.velocity.x > 0)) {

			Debug.Log("CheckpointRight entered");
			Debug.Log(other.transform.position);
			Debug.Log(other.transform.name);
			LevelManager.instance.createNewLevelPart(other.transform.position, Mathf.Sign(rigidbody2D.velocity.x));
			
			// deactivate Checkpoint afterwards
			other.enabled = false;

		}
		if (other.tag.Equals("CheckpointLeft") && (rigidbody2D.velocity.x < 0)) {
			
			Debug.Log("CheckpointLeft entered");
			Debug.Log(other.transform.position);
			Debug.Log(other.transform.name);
			LevelManager.instance.createNewLevelPart(other.transform.position, Mathf.Sign(rigidbody2D.velocity.x));
			
			// deactivate Checkpoint afterwards
			other.enabled = false;
			
		}
	}

	void reset() {
		// reset the player
		GameObject tmp = this.gameObject;
		GameObject player = (GameObject) Instantiate(playerPrefab, startPosition, Quaternion.identity);
		LevelManager.instance.player = player;
		Destroy(tmp);
	}
}
