using UnityEngine;
using UnityEngine.SceneManagement;
using System;

// Singleton handling VR inputs
public class VRInput : MonoBehaviour {
	public static VRInput instance;
	public bool enableInput = true;

	void Awake() {
		if (instance == null) {
			instance = this;
			enableInput = true;
		} else {
			Debug.Log("Destroying GameObject with duplicate VRInput: " + gameObject.name);
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (enableInput && OVRManager.instance.isUserPresent)
			CheckInput();

		#if UNITY_EDITOR
		if (enableInput)
			CheckInput();
		#endif
	}

	private void CheckInput() {
		// Check for a press down of the fire button
		if (Input.GetButtonDown ("Fire1")) {
			EventManager.instance.TriggerEvent (EventManager.INPUT_DOWN);
		}
	}
}
