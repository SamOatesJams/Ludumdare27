using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {
	
	/**
	    Local Enums
	*/
	
	private enum JumpState {
		Landed,
		Jumping
	};
	
	/**
		Public variables 
	*/
	public int m_health = 10;					//!< The health of the player
	public float m_speedModifier = 200.0f;		//!< A scalar to apply to all movement speeds
	public float m_jumpHeight = 20.0f;			//!< The upwards force of the player jump
	
	/**
		Private variables 
	*/	

	private JumpState m_jumpState = JumpState.Landed;
	private float m_glitchTime = 0.0f;
	
	
	/**
		\brief Use this for initialization
	*/
	void Start () {
		
	}
	
	/**
		\brief Update is called once per frame
	*/
	void Update () {

		float leftRight = Input.GetAxis("Horizontal") * (m_jumpState == JumpState.Jumping ? 0.5f : 1.0f);
		
		float upDown = Input.GetAxis("Vertical");
		if (m_jumpState == JumpState.Landed)
		{
			if (upDown > 0.1f)
			{
				m_jumpState = JumpState.Jumping;
				upDown = m_jumpHeight;
			}
			else if (m_glitchTime == 0.0f && upDown < -0.9f)
			{
				m_glitchTime = Time.time;
				this.transform.position = this.transform.position + new Vector3(0.0f, -200.0f, 0.0f); 
			}
		}
		else
		{
			upDown = 0.0f;	
		}
	
		if (m_glitchTime != 0.0f && Time.time - m_glitchTime >= 10.0f)
		{
			m_glitchTime = 0.0f;
			this.transform.position = this.transform.position + new Vector3(0.0f, 200.0f, 0.0f);
		}
		
		this.rigidbody.AddForce(new Vector3(leftRight * m_speedModifier, upDown * m_speedModifier, 0.0f));
		
	}
	
	/**
		\brief Called when the player collides with something
	*/
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (Vector3.Dot(contact.normal, Vector3.up) > 0.75f)
			{
				m_jumpState = JumpState.Landed;
				return;
			}
		}
	}
	
	public float GetGlitchTimeRemaining() {
		return m_glitchTime != 0.0f ? Time.time - m_glitchTime : 0.0f;
	}
}
