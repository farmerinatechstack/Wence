using UnityEngine;
using System;

// Attach to any GameObject which reacts to gaze inputs 
public class VRInteractible : MonoBehaviour {
	public event Action OnEnter;
	public event Action OnExit;
	public event Action OnDown;

	protected bool entered;
	public bool isEntered {
		get { return entered; }
	}

	void OnDisable() {
		EventManager.instance.StopListening (EventManager.INPUT_DOWN, Down);
	}

	void Start() {
		EventManager.instance.StartListening (EventManager.INPUT_DOWN, Down);
	}

	public void Enter() {
		entered = true;

		if (OnEnter != null)
			OnEnter();
	}

	public void Exit() {
		entered = false;

		if (OnExit != null) 
			OnExit();
	}

	public void Down() {
		if (OnDown != null)
			OnDown ();
	}
}
