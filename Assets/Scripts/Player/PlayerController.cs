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
	
	const int FULLHEALTH = 10;
	
	/**
		Public variables 
	*/
	public int m_health = FULLHEALTH;			//!< The health of the player
	public float m_speedModifier = 200.0f;		//!< A scalar to apply to all movement speeds
	public float m_jumpHeight = 20.0f;			//!< The upwards force of the player jump
	
	/**
		Private variables 
	*/	

	private JumpState m_jumpState = JumpState.Landed;
	private float m_jumpTime = 0.0f;
	private float m_glitchTime = 0.0f;
	private Vector3 m_spawnPosition = Vector3.zero;
	private float m_spawnTime = 0.0f;
	
	
	/**
		\brief Use this for initialization
	*/
	void Start () {
		m_spawnPosition = this.transform.position;
	}
	
	/**
		\brief Update is called once per frame
	*/
	void Update () {
		
		if (m_spawnTime != 0.0f && Time.time - m_spawnTime < 0.5f)
			return;
		
		float leftRight = Input.GetAxis("Horizontal") * (m_jumpState == JumpState.Jumping ? 0.5f : 1.0f);
		
		float upDown = Input.GetAxis("Vertical");
		if (m_jumpState == JumpState.Landed)
		{
			if (m_jumpTime == 0.0f && upDown > 0.1f)
			{
				m_jumpState = JumpState.Jumping;
				upDown = m_jumpHeight;
				m_jumpTime = Time.time;
			}
			else if (m_glitchTime == 0.0f && upDown < -0.9f)
			{
				m_glitchTime = Time.time;
				this.transform.position = this.transform.position + new Vector3(0.0f, -200.0f, 0.0f); 
				Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.0f, -200.0f, 0.0f); 
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
			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.0f, 200.0f, 0.0f); 
		}
		
		if (m_jumpTime != 0.0f && Time.time - m_jumpTime >= 0.2f)
		{
			m_jumpTime = 0.0f;
		}
		
		this.rigidbody.AddForce(new Vector3(leftRight * m_speedModifier, upDown * m_speedModifier, 0.0f));
		Camera.main.transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);		
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
				if (m_jumpState == JumpState.Jumping)
				{
					m_jumpState = JumpState.Landed;
					m_jumpTime = Time.time;
					return;
				}
			}
		}
	}
	
	/**
		\brief Get the glitch time remaining
	*/
	public float GetGlitchTimeRemaining() {
		return m_glitchTime != 0.0f ? Time.time - m_glitchTime : 0.0f;
	}
	
	/**
		\brief Called when the player enters a trigger
	*/
	void OnTriggerEnter(Collider other) {
		
		// Handle water (should this be here or in a water script?)
		if (other.tag == "WaterBlock")
		{
			KillPlayer();
		}
	}
	
	public void KillPlayer()
	{
		this.transform.position = m_spawnPosition;
		Camera.main.transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
		this.rigidbody.velocity = Vector3.zero;
		
		m_health = FULLHEALTH;
		
		m_jumpState = JumpState.Landed;
		m_jumpTime = 0.0f;
				
		if (m_glitchTime != 0.0f)
		{
			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.0f, 200.0f, 0.0f); 
			m_glitchTime = 0.0f;
		}
		
		m_spawnTime = Time.time;
	}
}
