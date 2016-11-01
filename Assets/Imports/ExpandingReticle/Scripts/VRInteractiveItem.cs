using UnityEngine;
using System;

// Attach to any GameObject which reacts to gaze inputs 
public class VRInteractiveItem : MonoBehaviour {
	public event Action OnEnter;
	public event Action OnExit;
	public event Action OnDown;

	private bool inGaze = false;
	public bool isInGaze {
		get { return inGaze; }
	}

	public void Enter() {
		Debug.Log ("Gaze entered gameObject: " + gameObject.name);
		inGaze = true;

		if (OnEnter != null)
			OnEnter();
	}

	public void Exit() {
		Debug.Log ("Gaze exited gameObject: " + gameObject.name);
		inGaze = false;

		if (OnExit != null) 
			OnExit();
	}

	public void Down() {
		if (OnDown != null)
			OnDown ();
	}
}
