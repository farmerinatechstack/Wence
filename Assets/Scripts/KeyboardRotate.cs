﻿using UnityEngine;
using System.Collections;

public class KeyboardRotate : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");


		gameObject.transform.Rotate(new Vector3(-vertical, horizontal, 0));
	}
}
