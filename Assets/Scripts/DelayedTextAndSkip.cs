using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DelayedTextAndSkip : MonoBehaviour {
	[SerializeField] Text textToEnable;
	[SerializeField] ManageSceneView sceneManager;

	private bool readyToSkip = false;

	// Use this for initialization
	void Start () {
		Invoke ("EnableTextSkip", 20f);
	}

	void Update() {
		if (readyToSkip && Input.GetButtonDown ("Fire1")) {
			sceneManager.TransitionEarly ();
		}
	}
	
	void EnableTextSkip() {
		textToEnable.enabled = true;
		readyToSkip = true;
	}
}
