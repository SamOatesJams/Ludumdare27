﻿using UnityEngine;
using System.Collections;

public class Spring : MonoBehaviour {
	
	public Vector3 m_direction = Vector3.up;
	public float m_force = 1000.0f;
	
	private AnimatedTexture m_texture = null;
		
	// Use this for initialization
	void Start () {
		m_texture = this.GetComponent<AnimatedTexture>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnTriggerEnter(Collider other) {
	
		if (other.tag != "Player")
			return;
		
		m_texture.m_play = true;
		
		Rigidbody rb = other.GetComponent<Rigidbody>();
		rb.AddForce(m_direction * m_force, ForceMode.Impulse);
		
	}
}
