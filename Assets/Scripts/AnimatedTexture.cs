using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {
	
	public int m_numberOfColumns = 4;
	public int m_frameRate = 10;
	public bool m_randomStartFrame = false;
	public bool m_pingPong = false;
	
	private int m_currentColumn = 0;
	private float m_lastFrameTime = 0.0f;
	private int m_nextFrame = 1;
	
	// Use this for initialization
	void Start () {
	
		if (m_randomStartFrame)
		{
			m_currentColumn = Random.Range(0, m_numberOfColumns - 1);
		}
		m_lastFrameTime = Time.time;
		
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Time.time - m_lastFrameTime >= (1.0f / m_frameRate)) {
			m_currentColumn += m_nextFrame;
			if (m_currentColumn > (m_numberOfColumns - 1) || m_currentColumn < 0) {
				if (!m_pingPong) {
					m_currentColumn = 0;
				} else {
					m_nextFrame = m_nextFrame * -1;
					m_currentColumn += m_nextFrame;
				}				
			}
			m_lastFrameTime = Time.time;
		
			Vector2 offset = new Vector2(m_currentColumn * (1.0f / m_numberOfColumns), 0.0f);
			this.renderer.material.mainTextureOffset = offset;
		}
		
	}
}
