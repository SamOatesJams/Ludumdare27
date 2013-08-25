using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour {
	
	public GUISkin m_skin = null;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {
		
		GUI.skin = m_skin;
		bool pressed = GUI.Button(new Rect((Screen.width * 0.5f) - ((Screen.width * 0.25f) * 0.5f), (Screen.height * 0.5f) - ((Screen.height * 0.125f) * 0.5f), Screen.width * 0.25f, (Screen.height * 0.25f) * 0.5f), "Escape");
		if (pressed)
		{
			Application.LoadLevel("Level-001");	
		}
	}
}
