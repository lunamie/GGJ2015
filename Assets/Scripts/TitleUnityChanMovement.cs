using UnityEngine;
using System.Collections;

// Hong Linh Thai
// Script which moves Unity Chan in the title screen

[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class TitleUnityChanMovement : MonoBehaviour {

	public float maxSpeed = 10f;
	public float jumpPower = 1000f;
	public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);

	public LayerMask whatIsGround;

	private Animator m_animator;
	private BoxCollider2D m_boxcollier2D;
	private Rigidbody2D m_rigidbody2D;
	private bool m_isGround;
	private const float m_centerY = 1.5f;

	private State m_state = State.Normal;

	private float direction;

	void Reset()
	{
		Awake();
		
		// UnityChan2DController
		maxSpeed = 10f;
		jumpPower = 1000;
		backwardForce = new Vector2(-4.5f, 5.4f);
		whatIsGround = 1 << LayerMask.NameToLayer("Ground");
		
		// Transform
		transform.localScale = new Vector3(1, 1, 1);
		
		// Rigidbody2D
		m_rigidbody2D.gravityScale = 3.5f;
		m_rigidbody2D.fixedAngle = true;
		
		// BoxCollider2D
		m_boxcollier2D.size = new Vector2(1, 2.5f);
		m_boxcollier2D.center = new Vector2(0, -0.25f);
		
		// Animator
		m_animator.applyRootMotion = false;
	}

	// Use this for initialization
	void Awake () {
		m_animator = GetComponent<Animator>();
		m_boxcollier2D = GetComponent<BoxCollider2D>();
		m_rigidbody2D = GetComponent<Rigidbody2D>();
		direction = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
		Move(direction, false);

		// change direction if would fall down
		Vector2 position = new Vector2( transform.position.x+direction*5, transform.position.y );
		Debug.DrawRay(transform.position, new Vector3(direction, 0, 0), Color.red, 0.1f);
		
		RaycastHit2D hit = Physics2D.Raycast( position, -Vector2.up, 3.0f, 1 << LayerMask.NameToLayer("Ground"));
		if (hit.collider == null) {
			direction = direction*-1;
		}
	}

	void Move(float move, bool jump)
	{
		if (Mathf.Abs(move) > 0)
		{
			Quaternion rot = transform.rotation;
			transform.rotation = Quaternion.Euler(rot.x, Mathf.Sign(move) == 1 ? 0 : 180, rot.z);
		}
		
		m_rigidbody2D.velocity = new Vector2(move * maxSpeed, m_rigidbody2D.velocity.y);
		
		m_animator.SetFloat("Horizontal", move);
		m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
		
		m_animator.SetBool("isGround", m_isGround);
		
		if (jump && m_isGround)
		{
			m_animator.SetTrigger("Jump");
			SendMessage("Jump", SendMessageOptions.DontRequireReceiver);
			m_rigidbody2D.AddForce(Vector2.up * jumpPower);
		}
	}
	
	void FixedUpdate()
	{
		Vector2 pos = transform.position;
		Vector2 groundCheck = new Vector2(pos.x, pos.y - (m_centerY * transform.localScale.y));
		Vector2 groundArea = new Vector2(m_boxcollier2D.size.x * 0.49f, 0.05f);
		
		m_isGround = Physics2D.OverlapArea(groundCheck + groundArea, groundCheck - groundArea, whatIsGround);
		m_animator.SetBool("isGround", m_isGround);
	}

	enum State
	{
		Normal,
		Damaged,
		Invincible,
	}
}
