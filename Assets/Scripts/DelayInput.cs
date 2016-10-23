using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VR;
using System.Collections;

public class DelayInput : MonoBehaviour {
	[SerializeField] Text textToEnable;

	void Start () {
		VRInput.instance.enableInput = false;
		textToEnable.enabled = false;
		Invoke ("EnableInput", 15f);
	}

	void Update() {
		if (!OVRManager.instance.isUserPresent) {
			EnableSwap ();
		}
	}

	void EnableSwap() {
		textToEnable.enabled = true;
		VRInput.instance.enableInput = true;
	}
}
