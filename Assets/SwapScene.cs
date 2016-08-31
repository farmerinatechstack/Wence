using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwapScene : MonoBehaviour {
	[SerializeField] GeneralFader fader;

	private void Update() {
		if (Input.GetButtonDown ("Fire1")) { 	// Touchpad down
			// Swap scene
			fader.FadeTo();
			StartCoroutine (Swap());
		}
	}

	IEnumerator Swap() {
		yield return new WaitForSeconds (fader.fadeTime);
		SceneManager.LoadScene ("WenceVirtual");
	}
}
