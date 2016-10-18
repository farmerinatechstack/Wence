using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;

public class DelayedTextAndSkip : MonoBehaviour {
	[SerializeField] AudioSource chimeSource;
	[SerializeField] SwapScene swapper;
	[SerializeField] Text textToEnable;

	// Use this for initialization
	void Start () {
		textToEnable.enabled = false;
		Invoke ("TimeElapsed", 25f);
	}

	void Update() {
		if (VRDevice.isPresent && Input.GetButtonDown ("Fire1") && textToEnable.enabled) {
			swapper.ExecuteSwap ();
		}
	}

	void OnApplicationPause(bool pauseStatus) {
		textToEnable.enabled = true;
	}

	void TimeElapsed() {
		textToEnable.enabled = true;
	}
}
