using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TapToSwitchScene : MonoBehaviour {
	[SerializeField] string destinationSceneName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Fire1")) {
			SceneManager.LoadScene (destinationSceneName);
		}
	}
}
