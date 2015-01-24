using System.Collections;
using UnityEngine;
[RequireComponent(typeof(Animator), typeof(Rigidbody2D), typeof(BoxCollider2D))]
public class UnityChan2DController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpPower = 1000f;
    public Vector2 backwardForce = new Vector2(-4.5f, 5.4f);

    public LayerMask whatIsGround;

    private Animator m_animator;
    private BoxCollider2D m_boxcollier2D;
    private Rigidbody2D m_rigidbody2D;
    private bool m_isGround;
    private const float m_centerY = 1.5f;
	PhotonView m_PhotonView;

    private State m_state = State.Normal;

	public GameObject playerPrefab;
	public float falling_treshhold;

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

    void Awake()
    {
        m_animator = GetComponent<Animator>();
        m_boxcollier2D = GetComponent<BoxCollider2D>();
        m_rigidbody2D = GetComponent<Rigidbody2D>();
		m_PhotonView = GetComponent<PhotonView>();

		PhotonNetwork.sendRate = 25;
		PhotonNetwork.sendRateOnSerialize = 25;
    }

    void Update()
    {
		if (m_state != State.Damaged && (m_PhotonView == null || PhotonNetwork.connected == false))
		{
			float x = Input.GetAxis("Horizontal");
			bool jump = Input.GetButtonDown("Jump");
			Move(x, jump);
		}

        if (m_state != State.Damaged && m_PhotonView.isMine == true)
        {
            float x = Input.GetAxis("Horizontal");
            Move(x, false);
        }

		if(transform.position.y < falling_treshhold) {
			reset ();
			
			// reset the enemies
			LevelManager.instance.resetLevel();
		}

		if(m_PhotonView.isMine == false && Input.GetButtonDown("Jump") && PhotonNetwork.connected) {
			m_PhotonView.RPC( "DoJump", PhotonTargets.Others );
		}
    }

	[RPC]
	public void DoJump()
	{
		m_animator.SetTrigger("Jump");
		Move(0.0f, true);
	}

	[RPC]
	public void animateMove(float move) {
		m_animator.SetFloat("Horizontal", move);
		m_animator.SetFloat("Vertical", m_rigidbody2D.velocity.y);
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

		if(PhotonNetwork.connected)
			m_PhotonView.RPC("animateMove", PhotonTargets.Others, move);
        
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

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "DamageObject" && m_state == State.Normal)
        {
            m_state = State.Damaged;
            StartCoroutine(INTERNAL_OnDamage());
        }
    }

    IEnumerator INTERNAL_OnDamage()
    {
        m_animator.Play(m_isGround ? "Damage" : "AirDamage");
        m_animator.Play("Idle");

        SendMessage("OnDamage", SendMessageOptions.DontRequireReceiver);

        m_rigidbody2D.velocity = new Vector2(transform.right.x * backwardForce.x, transform.up.y * backwardForce.y);

        yield return new WaitForSeconds(.2f);

        while (m_isGround == false)
        {
            yield return new WaitForFixedUpdate();
        }
        m_animator.SetTrigger("Invincible Mode");
        m_state = State.Invincible;
    }

    void OnFinishedInvincibleMode()
    {
        m_state = State.Normal;
    }

    enum State
    {
        Normal,
        Damaged,
        Invincible,
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
		transform.position = LevelManager.instance.startPosition;
		rigidbody2D.velocity = Vector2.zero;
		rigidbody2D.angularVelocity = 0;
		gameObject.SetActive(true);
	}
}
