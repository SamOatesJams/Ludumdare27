using UnityEngine;
using System.Collections;

public class AnimatedTexture : MonoBehaviour {
	
	public int m_numberOfColumns = 4;
	public int m_frameRate = 10;
	public bool m_play = true;
	public bool m_loop = true;
	public bool m_randomStartFrame = false;
	public bool m_pingPong = false;
	public bool m_random = false;
	
	protected int m_currentColumn = 0;
	protected float m_lastFrameTime = 0.0f;
	protected int m_nextFrame = 1;
	
	// Use this for initialization
	public void Start () {
	
		if (m_randomStartFrame)
		{
			m_currentColumn = Random.Range(0, m_numberOfColumns - 1);
		}
		m_lastFrameTime = Time.time;
		
		Vector2 scale = new Vector2(1.0f / m_numberOfColumns, this.renderer.material.mainTextureScale.y);
		this.renderer.material.mainTextureScale = scale;
		
	}
	
	// Update is called once per frame
	public void Update () {
	
		if (!m_play)
			return;
		
		if (Time.time - m_lastFrameTime >= (1.0f / m_frameRate)) {
			
			if (!m_random) {
				m_currentColumn += m_nextFrame;
			} else {
				m_currentColumn = Random.Range(0, m_numberOfColumns);
			}
			
			if (m_currentColumn > (m_numberOfColumns - 1) || m_currentColumn < 0) {
				if (!m_pingPong) {
					m_currentColumn = 0;
					if (!m_loop) {
						m_play = false;
						return;	
					}
				} else {
					m_nextFrame = m_nextFrame * -1;
					m_currentColumn += m_nextFrame;
					if (!m_loop && m_currentColumn <= 0) {
						m_currentColumn = 0;
						m_play = false;
						return;	
					}
				}				
			}
			m_lastFrameTime = Time.time;
		
			Vector2 offset = new Vector2(m_currentColumn * (1.0f / m_numberOfColumns), this.renderer.material.mainTextureOffset.y);
			this.renderer.material.mainTextureOffset = offset;
		}
		
	}
}
