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
	
	public float m_speedModifier = 200.0f;		//!< A scalar to apply to all movement speeds
	public float m_jumpHeight = 20.0f;			//!< The upwards force of the player jump
	
	public AudioClip[] m_jumpSounds = null;
	public AudioClip[] m_deadSounds = null;
	
	/**
		Private variables 
	*/	
	private JumpState m_jumpState = JumpState.Landed;
	private float m_jumpTime = 0.0f;
	private float m_glitchTime = 0.0f;
	private Vector3 m_spawnPosition = Vector3.zero;
	private float m_spawnTime = 0.0f;
	private bool m_doGlitch = false;
	private AnimatedPlayerTexture m_texture = null;
	
	private GameObject m_blackQuad = null;
	private float m_blackTime = 0.0f;
	
	private AudioSource m_glitchSound = null;
	private AudioSource m_audio = null;
	
	private static int m_lives = 3;
	private static int m_score = 0;	
	private int m_levelStartScore = 0;
	
	/**
		\brief Use this for initialization
	*/
	void Start () {
		m_spawnPosition = this.transform.position;
		m_texture = this.GetComponentInChildren<AnimatedPlayerTexture>();
		
		Transform quad = Camera.main.transform.FindChild("black-quad");
		if (quad)
		{
			m_blackQuad = quad.gameObject;
			m_blackQuad.SetActive(false);
		}
		
		Transform sound = this.transform.FindChild("glitch-sound");
		if (sound)
		{
			m_glitchSound = sound.GetComponent<AudioSource>();
			m_audio = this.GetComponent<AudioSource>();
		}
		
		m_score += 10000;
		m_levelStartScore = m_score;
	}
	
	/**
		\brief Update is called once per frame
	*/
	void Update () {
		
		if (!m_texture)
			return;
		
		if (m_spawnTime != 0.0f && Time.time - m_spawnTime < 2.0f)
			return;
		else if (m_spawnTime != 0.0f && Time.time - m_spawnTime >= 2.0f)
		{
			Camera.main.GetComponent<AudioSource>().mute = false;
			m_audio.pitch = 1.0f;
			m_spawnTime = 0.0f;
		}
				
		if (m_blackTime != 0.0f)
		{
			if (Time.time - m_blackTime >= 2.0f)
			{
				m_blackTime = 0.0f;
				m_blackQuad.SetActive(false);
			}
			else if (Random.Range(0, 4) == 0)
			{
				m_blackQuad.SetActive(!m_blackQuad.activeSelf);
				m_blackQuad.renderer.material.mainTextureOffset = new Vector2(0, Random.Range(0, 100) * 0.01f);
			}
		}
		
		m_score--;
		if (m_score < 0)
			m_score = 0;
		
		string currentAnimation = "idle";
		int frameRate = 4;
		float leftRight = Input.GetAxis("Horizontal") * (m_jumpState == JumpState.Jumping ? 0.5f : 1.0f);
		
		if (leftRight != 0.0f)
		{
			currentAnimation = "walk";
			frameRate = 7;
		}
		
		float upDown = Input.GetAxis("Vertical");
		float upPower = 0.0f;
		if (m_jumpState == JumpState.Landed)
		{
			if (m_jumpTime == 0.0f && upDown > 0.1f)
			{
				m_doGlitch = false;
				m_jumpState = JumpState.Jumping;
				upPower = m_jumpHeight;
				m_jumpTime = Time.time;
								
				AudioClip clip = m_jumpSounds[Random.Range(0, m_jumpSounds.Length)];
				m_audio.clip = clip;
				m_audio.Play();
			}
		}
		
		if (m_jumpState == JumpState.Jumping)
		{
			if (!m_doGlitch)
			{
				currentAnimation = "jump";	
				frameRate = 6;
			}
			else
			{
				currentAnimation = "glitch";	
				frameRate = 12;	
			}
		}
		
		if (m_glitchTime != 0.0f)
		{
			frameRate *= 20;
			if (Random.Range(0, 2) == 0)
			{
				m_blackQuad.SetActive(!m_blackQuad.activeSelf);
				m_blackQuad.renderer.material.mainTextureOffset = new Vector2(0, Random.Range(0, 100) * 0.01f);
			}	
		}

		if (m_glitchTime == 0.0f && upDown < -0.1f && m_jumpState == JumpState.Jumping)
		{
			m_doGlitch = true;	
			m_blackQuad.SetActive(true);
			m_blackTime = Time.time;
		}
	
		if (m_glitchTime != 0.0f && Time.time - m_glitchTime >= 10.0f)
		{
			m_doGlitch = false;
			m_glitchTime = 0.0f;
			this.transform.position = this.transform.position + new Vector3(0.0f, 200.0f, 0.0f);
			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.0f, 200.0f, 0.0f); 
			m_blackQuad.SetActive(true);
			m_blackTime = Time.time;
			m_audio.pitch = 1.0f;	
		}
		
		if (m_jumpTime != 0.0f && Time.time - m_jumpTime >= 0.2f)
		{
			m_jumpTime = 0.0f;
		}

		m_texture.m_frameRate = frameRate;
		m_texture.SetCurrentAnimation(currentAnimation, leftRight < 0.0f);
		
		leftRight = leftRight < 0.0f ? -1.0f : leftRight > 0.0f ? 1.0f : 0.0f;
		this.rigidbody.AddForce(new Vector3(leftRight * (m_speedModifier * (m_glitchTime == 0.0f ? 1.0f : 1.5f)), upPower * (800.0f * (m_glitchTime == 0.0f ? 1.0f : 1.1f)), 0.0f));
		Camera.main.transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);		
	}
	
	/**
		\brief Called when the player collides with something
	*/
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts)
		{
			if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
			{
				if (m_jumpState == JumpState.Jumping)
				{
					m_jumpState = JumpState.Landed;
					m_jumpTime = Time.time;
					if (m_doGlitch) 
					{
						m_glitchTime = Time.time;
						this.transform.position = this.transform.position + new Vector3(0.0f, -200.0f, 0.0f); 
						Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.0f, -200.0f, 0.0f); 
						m_doGlitch = false;	
						m_glitchSound.Play();
						m_audio.pitch = 2.0f;
					}
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
		AudioClip clip = m_deadSounds[Random.Range(0, m_deadSounds.Length)];
		m_audio.clip = clip;
		m_audio.Play();
		
		m_lives--;
		if (m_lives < 0)
		{
			Application.LoadLevel("Win");	
		}
		
		m_levelStartScore -= 2500;
		if (m_levelStartScore < 0) m_levelStartScore = 0;
		m_score = m_levelStartScore;
		
		Camera.main.GetComponent<AudioSource>().mute = true;
		
		this.transform.position = m_spawnPosition;
		Camera.main.transform.position = new Vector3(this.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z);
		this.rigidbody.velocity = Vector3.zero;
		
		m_jumpState = JumpState.Landed;
		m_jumpTime = 0.0f;
				
		if (m_glitchTime != 0.0f)
		{
			Camera.main.transform.position = Camera.main.transform.position + new Vector3(0.0f, 200.0f, 0.0f); 
			m_glitchTime = 0.0f;
		}
		
		m_texture.m_frameRate = 4;
		m_texture.SetCurrentAnimation("idle", false);
		m_blackQuad.SetActive(false);
		m_blackTime = 0.0f;
		
		m_glitchSound.Stop();
		
		m_spawnTime = Time.time;
	}
	
	public bool IsDead()
	{
		return m_spawnTime != 0.0f;
	}
	
	public string GetScoreString()
	{
		return m_score.ToString("0x00000000#");
	}
	
	public int GetLives()
	{
		return m_lives;	
	}
	
	public void Reset()
	{
		m_lives = 3;
		m_score = 0;
	}
}
