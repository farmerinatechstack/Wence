using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ResetManager: MonoBehaviour {
	// Use this for initialization
	void Start () {
		EventManager.instance.StartListening (EventManager.CHANGING_SCENE, Reset);
	}
	
	void Reset() {
		if (SceneManager.GetActiveScene().name == "WhenceEnd") {
			Destroy (gameObject);
		}
	}
}
