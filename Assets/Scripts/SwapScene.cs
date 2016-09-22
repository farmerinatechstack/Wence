using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwapScene : MonoBehaviour {
	[SerializeField] FadeTransition trans;
	[SerializeField] GameObject fader;

	AsyncOperation asyncLoad;

	private void Start() {
		StartCoroutine (ActivateFader ());

		asyncLoad = Application.LoadLevelAsync("WenceVideo");
		asyncLoad.allowSceneActivation = false;
	}

	private void Update() {
		if (Input.GetButtonDown ("Fire1")) { 	// Touchpad down
			StartCoroutine (Swap());
		}
	}

	IEnumerator ActivateFader() {
		trans.FadeIn ();
		yield return new WaitForSeconds (trans.fadeTime);

		fader.SetActive (false);
	}

	IEnumerator Swap() {
		while (!asyncLoad.isDone) {
			float progress = asyncLoad.progress / 0.9f;
			yield return null;

			if (Mathf.Approximately(asyncLoad.progress, 0.9f)) {
				fader.SetActive (true);
				trans.FadeOut ();
				yield return new WaitForSeconds (trans.fadeTime);
				asyncLoad.allowSceneActivation = true;
			}
		}
	}
}