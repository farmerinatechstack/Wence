using UnityEngine;
using System.Collections;

public class ResetExperience : MonoBehaviour {

	void Start() {
		Invoke ("DestroyManagers", 15f);
	}

	// Destroys all GameObjects marked to be destroyed on restart
	void OnDestroy () {
		GameObject[] restarts = GameObject.FindGameObjectsWithTag ("DestroyOnRestart");

		foreach (GameObject obj in restarts) {
			Destroy(obj);
		}
	}

	void DestroyManagers() {
		Destroy (GameObject.FindGameObjectWithTag ("Manager"));
	}
}
