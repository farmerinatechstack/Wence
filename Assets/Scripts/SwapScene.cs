using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SwapScene : MonoBehaviour {
	[SerializeField] AudioSource chimeSource;
	[SerializeField] FadeTransition trans;
	[SerializeField] GameObject fader;
	[SerializeField] string sceneName;

	AsyncOperation asyncLoad;

	private void Start() {
		if (trans != null && fader != null) {
			StartCoroutine (ActivateFader ());
		}

		asyncLoad = SceneManager.LoadSceneAsync(sceneName);
		asyncLoad.allowSceneActivation = false;
	}

	private void Update() {
		if (Input.GetButtonDown ("Fire1")) { 	// Touchpad down
			StartCoroutine (Swap());
			chimeSource.Play ();
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
				if (trans != null && fader != null) {
					fader.SetActive (true);
					trans.FadeOut ();
					yield return new WaitForSeconds (trans.fadeTime);
				}
				asyncLoad.allowSceneActivation = true;
			}
		}
	}
}