using UnityEngine;
using System.Collections;

public class PlayerGUI : MonoBehaviour {
	
	public Texture2D m_glitchProgressbarTexture = null;
	
	private PlayerController m_player = null;
	
	// Use this for initialization
	void Start () {
		m_player = this.transform.parent.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		float glitchProgress = 1.0f - (m_player.GetGlitchTimeRemaining() / 10.0f);
		
		if (glitchProgress == 1.0f)
			return;
		
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
