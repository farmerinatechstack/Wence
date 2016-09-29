using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DelayedSwap : MonoBehaviour {
	[SerializeField] string destinationSceneName;
	[SerializeField] float delayTime;

	[SerializeField] FadeTransition trans;
	[SerializeField] GameObject fader;

	AsyncOperation asyncLoad;

	private void Start() {
		StartCoroutine (ActivateFader ());
		StartCoroutine (WaitToSwap ());

		asyncLoad = SceneManager.LoadSceneAsync(destinationSceneName);
		asyncLoad.allowSceneActivation = false;
	}

	IEnumerator ActivateFader() {
		trans.FadeIn ();
		yield return new WaitForSeconds (trans.fadeTime);

		fader.SetActive (false);
	}

	IEnumerator WaitToSwap() {
		yield return new WaitForSeconds (delayTime);
		StartCoroutine (Swap ());
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
