using UnityEngine;
using System.Collections;

public class EndPortal : MonoBehaviour {
	
	public float TeleportDelay = 2.0f;
	public string NextLevelName = null;
	
	private float m_portalTime = 0.0f;
	
	// Use this for initialization
	void Start () {
	
		if (NextLevelName == null || NextLevelName.Length == 0) {
			Debug.LogWarning("Portal has no destination");			
			this.enabled = false;
		}
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_portalTime != 0.0f && Time.time - m_portalTime >= TeleportDelay)
		{
			m_portalTime = 0.0f;
			// Load next level
			Application.LoadLevel(NextLevelName);
		}		
	}
	
	void OnTriggerEnter(Collider other) {
		
		// We only care about the player
		if (other.tag != "Player")
			return;
		
		m_portalTime = Time.time;
		
	}
	
	void OnTriggerExit(Collider other) {
		
		// We only care about the player
		if (other.tag != "Player")
			return;
		
		m_portalTime = 0.0f;
		
	}
}
