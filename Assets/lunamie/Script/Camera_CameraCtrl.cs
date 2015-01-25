using UnityEngine;
using System.Collections;

public class Camera_CameraCtrl : MonoBehaviour {
	PhotonView m_PhotonView;
	Vector3 m_velocity = Vector3.zero;
	float max   = 2.0f;
	float min	= 1.0f;
	float decay = 0.9999f;
	float force = 0.25f;
	// Use this for initialization
	void Awake()
	{
		m_PhotonView = GetComponent<PhotonView>();
	}

	void Start () {
	}

	// Update is called once per frame
	void Update () {
		float InputX = Input.GetAxis("Horizontal");
		float InputY = Input.GetAxis("Vertical");
		if (m_PhotonView.isMine == false && PhotonNetwork.connected)
		{
			if (InputX!=0 || InputY!=0) {
				Vector3 v = new Vector3(InputX, InputY, 0);
				v.Normalize();
				m_velocity += new Vector3(v.x * force, v.y * force, 0);
				if(m_velocity.sqrMagnitude>max)
				{
					float m = max / m_velocity.sqrMagnitude;
					m_velocity.Normalize();
					m_velocity = new Vector3(m_velocity.x*m, m_velocity.y*m, 0);
				}
			}
			float sqrtm = m_velocity.sqrMagnitude;
			if (InputX==0 && InputY==0) {
				if(m_velocity.sqrMagnitude>min) {
					sqrtm *= decay;
				}
			}
			m_velocity.Normalize();
			m_velocity = new Vector3(m_velocity.x*sqrtm, m_velocity.y*sqrtm, 0);
			transform.position += m_velocity;
			m_PhotonView.RPC ("Move", PhotonTargets.Others, transform.position);
		}
	}
	
	[RPC]
	public void Move(Vector3 pos)
	{
		this.transform.position = pos;
	}
}
