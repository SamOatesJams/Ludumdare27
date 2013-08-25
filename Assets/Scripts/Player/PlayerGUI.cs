using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	
	public Texture2D m_glitchProgressbarTexture = null;
	public Texture2D m_deadTexture = null;
	public Texture2D m_deadTextureBlocked = null;
	
	public Texture2D m_lifeIcon = null;
	
	public Texture2D m_textureMute = null;
	public Texture2D m_textureSound = null;
	
	public GUISkin m_skin = null;
	
	private PlayerController m_player = null;
	
	// Use this for initialization
	void Start () {
		m_player = this.transform.parent.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		GUI.skin = m_skin;
		
		// mute button
		int soundsize = (int)(Screen.width * 0.05f);
		bool mute = GUI.Button(
			new Rect(
				Screen.width - soundsize - (Screen.width * 0.005f),
				Screen.height - soundsize - (Screen.width * 0.005f),
				soundsize, 
				soundsize
			), 
			AudioListener.pause ? m_textureMute : m_textureSound
		);
		
		if (mute)
		{
			AudioListener.pause = !AudioListener.pause;
		}
		
		// score display
		GUI.skin.label.fontSize = 32;
		if (Screen.height < 600)
		{
			GUI.skin.label.fontSize = 18;	
		}
		
		int scoreY = (int)(Screen.height - (Screen.height * 0.05f));
		GUI.Label(
			new Rect(
				Screen.width * 0.005f, 
				scoreY, 
				Screen.width, 
				Screen.height
			), 
			m_player.GetScoreString()
		);
		
		// life images
		int lifesize = (int)(Screen.width * 0.033f);
		for (int lifeIndex = 0; lifeIndex < m_player.GetLives(); ++lifeIndex)
		{
			GUI.DrawTexture(
				new Rect(
					Screen.width * 0.005f + ((lifesize + Screen.width * 0.005f) * lifeIndex), 
					scoreY - lifesize - (Screen.width * 0.005f),
					lifesize,
					lifesize
				), 
				m_lifeIcon
			);
		}

		// BSOD
		if (m_player.IsDead())
		{
			int size = Mathf.Min(Screen.width, Screen.height);
			int x = (int)((Screen.width - size) * 0.5f);
			int y = (int)((Screen.height - size) * 0.5f);			
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_deadTextureBlocked);	
			GUI.DrawTexture(new Rect(x, y, size, size), m_deadTexture);	
		}
			
		// Progress bar for glitch
		float glitchProgress = 1.0f - (m_player.GetGlitchTimeRemaining() / 10.0f);
		if (glitchProgress != 1.0f)
		{	
			// Draw the glitch remaing progress bar
			float width = Screen.width * 0.9f;
			
			Rect glitchBackgroundProgressbar = new Rect();
			glitchBackgroundProgressbar.x = (Screen.width - width) * 0.5f;
			glitchBackgroundProgressbar.y = Screen.height * 0.01f;
			glitchBackgroundProgressbar.width = width;
			glitchBackgroundProgressbar.height = Screen.height * 0.1f;
			
			Rect glitchProgressbar = glitchBackgroundProgressbar;
			glitchProgressbar.x += 2.0f;
			glitchProgressbar.y += 2.0f;
			glitchProgressbar.width -= 4.0f;
			glitchProgressbar.height -= 4.0f;
			
			glitchProgressbar.width *= glitchProgress;
			
			GUI.Box(glitchBackgroundProgressbar, "");
			GUI.DrawTexture(glitchProgressbar, m_glitchProgressbarTexture);
		}
			
	}
}
