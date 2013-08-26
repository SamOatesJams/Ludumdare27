using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	
	public GUISkin m_skin = null;
	
	public Texture2D m_textureMute = null;
	public Texture2D m_textureSound = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		GUI.skin = m_skin;
		
		int soundsize = (int)(Screen.width * 0.05f);
		bool mute = GUI.Button(
			new Rect(
				Screen.width - soundsize - (Screen.width * 0.005f),
				Screen.height - soundsize - (Screen.width * 0.005f),
				soundsize, 
				soundsize), 
				AudioListener.pause ? m_textureMute : m_textureSound
			);
		
		if (mute)
		{
			AudioListener.pause = !AudioListener.pause;
		}
		
		bool pressed = GUI.Button(
			new Rect(
				0, 
				(Screen.height * 0.5f) - ((Screen.height * 0.125f) * 0.5f), 
				Screen.width, 
				(Screen.height * 0.25f) * 0.5f
			), 
			"Escape"
		);
		
		if (pressed)
		{
			PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
			player.Reset();
			Application.LoadLevel("Level-001");	
		}
	}
}
