using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatedPlayerTexture : AnimatedTexture {
	
	public string m_currentAnimation = null;
	public int m_numberOfRows = 8;
		
	private Dictionary<string, Vector2> m_keyFrame = new Dictionary<string, Vector2>();
	private bool m_flippedHorizontal = false;
	
	// Use this for initialization
	void Start () {
		base.Start();
		
		m_keyFrame.Add("idle", new Vector2(0, 7));
		m_keyFrame.Add("walk", new Vector2(8, 13));
		
		if (!m_keyFrame.ContainsKey(m_currentAnimation))
		{
			Debug.LogError("We have no keyframe set called '" + m_currentAnimation + "'");
			m_currentAnimation = "idle";
		}
		
		Vector2 key = m_keyFrame[m_currentAnimation];
		m_currentColumn = (int)key.x;
		
		Vector2 scale = new Vector2(1.0f / m_numberOfColumns, 1.0f / m_numberOfRows);
		this.renderer.material.mainTextureScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time - m_lastFrameTime >= (1.0f / m_frameRate)) {
			m_lastFrameTime = Time.time;
			
			Vector2 key = m_keyFrame[m_currentAnimation];
			
			m_currentColumn += m_nextFrame;
			
			if ( m_currentColumn > key.y )
			{
				m_currentColumn = (int)key.x;
			}
			
			int xFrame = m_currentColumn % m_numberOfColumns + (m_flippedHorizontal ? 1 : 0);
			int yFrame = Mathf.FloorToInt(m_currentColumn / m_numberOfColumns);	
		
			Vector2 offset = new Vector2(xFrame * (1.0f / m_numberOfColumns), 1.0f - ((yFrame + 1) * (1.0f / m_numberOfRows)));
			this.renderer.material.mainTextureOffset = offset;
		}
		
	}
	
	//Set the current animation
	public void SetCurrentAnimation(string animation, bool flipHorizontal) {
		
		if (animation == m_currentAnimation && m_flippedHorizontal == flipHorizontal)
			return;
		
		m_currentAnimation = animation;
		Vector2 key = m_keyFrame[m_currentAnimation];
		m_currentColumn = (int)key.x;
		
		if (m_flippedHorizontal != flipHorizontal)
		{
			m_flippedHorizontal = flipHorizontal;
			Vector2 scale = new Vector2((m_flippedHorizontal ? -1.0f : 1.0f) / m_numberOfColumns, 1.0f / m_numberOfRows);
			this.renderer.material.mainTextureScale = scale;
		}
		
	}
}
