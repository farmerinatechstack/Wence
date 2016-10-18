using UnityEngine;
using System.Collections;

public class DontDestroy : MonoBehaviour {
	private static DontDestroy Instance;

	void Awake() {
		if (Instance) {
			Debug.LogWarning ("Destroying duplicate instance of DontDestroy");
		} else {
			Instance = this;
		}
	}

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
}
