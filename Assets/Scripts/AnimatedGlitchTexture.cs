using UnityEngine;
using System.Collections;

public class AnimatedGlitchTexture : AnimatedTexture {
	
	public Texture2D m_glitchTexture = null;
	public int m_numberOfColumnsInGlitchTexture = 8; 
	public int m_glitchChance = 10;
	
	private Texture m_originalTexture = null;
	
	// Use this for initialization
	void Start () {
		base.Start();
		m_glitchChance = Mathf.Clamp(m_glitchChance, 0, m_glitchChance);
		m_originalTexture = this.renderer.material.mainTexture;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Time.time - m_lastFrameTime >= (1.0f / m_frameRate)) {
			
			if (Random.Range(0, m_glitchChance) == 0)
			{
				// do a random glitch texture
				this.renderer.material.mainTexture = m_glitchTexture;
				
				int randomFrame = Random.Range(0, m_numberOfColumnsInGlitchTexture - 1);
				float frameWidth = 1.0f / m_numberOfColumnsInGlitchTexture;
				this.renderer.material.mainTextureScale = new Vector2(frameWidth, 1.0f);
				this.renderer.material.mainTextureOffset = new Vector2(frameWidth * randomFrame, 1.0f);
				m_lastFrameTime = Time.time;
			}	
			else
			{
				this.renderer.material.mainTexture = m_originalTexture;
				this.renderer.material.mainTextureScale = new Vector2(1.0f / m_numberOfColumns, 1.0f);
				base.Update();
			}
		}
		
	}
}
