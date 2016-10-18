using UnityEngine;
using System;

// Singleton handling VR inputs
public class VRInput : MonoBehaviour {
	public static VRInput Instance;
	public static bool enableInput = true;

	[SerializeField] AudioSource src;

	// VR inpus to subscribe to
	public static event Action OnInputDown;		// Called on press down of Fire1

	// Guarantee a singleton
	void Awake () {
		if (!Instance) {
			Instance = this;
		} else {
			Debug.LogError ("Multiple VRInputs detected, destroying gameObject: " + gameObject.name);
			Destroy (gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		if (enableInput)
			CheckInput();
	}

	private void CheckInput() {
		// Check for a press down of the fire button
		if (Input.GetButtonDown ("Fire1")) {
			if (OnInputDown != null)
				src.Play ();
				OnInputDown ();
		}
	}

	private void OnDestroy() {
		OnInputDown = null;
	}
}
