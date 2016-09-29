using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;

public class DelayedTextAndSkip : MonoBehaviour {
	[SerializeField] AudioSource chimeSource;
	[SerializeField] Text textToEnable;
	[SerializeField] ManageSceneView sceneManager;

	private bool readyToSkip = false;

	// Use this for initialization
	void Start () {
		Invoke ("EnableTextSkip", 10f);
	}

	void Update() {
		if (VRDevice.isPresent && readyToSkip && Input.GetButtonDown ("Fire1")) {
			sceneManager.TransitionEarly ();
			chimeSource.Play ();
		}
	}
	
	void EnableTextSkip() {
		textToEnable.enabled = true;
		readyToSkip = true;
	}
}
