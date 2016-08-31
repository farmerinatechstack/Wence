using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwapScene : MonoBehaviour {
	[SerializeField] GeneralFader fader;

	AsyncOperation async;

	private void Start() {
		StartCoroutine("load");
	}

	private void Update() {
		if (Input.GetButtonDown ("Fire1")) { 	// Touchpad down
			// Swap scene
			fader.FadeTo();
			StartCoroutine (Swap());
		}
	}

	IEnumerator Swap() {
		yield return new WaitForSeconds (fader.fadeTime);
		async.allowSceneActivation = true;
	}

	IEnumerator load() {
		async = Application.LoadLevelAsync("WenceVideo");
		async.allowSceneActivation = false;
		yield return async;
	}
}