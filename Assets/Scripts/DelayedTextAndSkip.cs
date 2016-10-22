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
		if (!OVRManager.instance.isUserPresent) {
			textToEnable.enabled = true;
		}

		if (OVRManager.instance.isUserPresent && Input.GetButtonDown("Fire1") && textToEnable.enabled) {
			chimeSource.Play ();
			swapper.ExecuteSwap ();
		}
	}

	void TimeElapsed() {
		textToEnable.enabled = true;
	}
}
