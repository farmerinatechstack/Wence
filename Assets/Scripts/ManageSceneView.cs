using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* Class: ManageSceneView
 * Manages scene transitions and the blind plane */
public class ManageSceneView : MonoBehaviour {
	[SerializeField] GameObject blindPlane;
	[SerializeField] string sceneName;
	[SerializeField] FadeTransition trans;
	[SerializeField] float transitionTime;

	private AsyncOperation asyncLoad;

	// Use this for initialization
	void Start () {
		asyncLoad = SceneManager.LoadSceneAsync (sceneName);
		asyncLoad.allowSceneActivation = false;

		StartCoroutine (FadeIn ());
		StartCoroutine (Swap ());
	}

	IEnumerator FadeIn() {
		trans.FadeIn ();
		yield return new WaitForSeconds (trans.fadeTime);

		blindPlane.SetActive (false);
	}

	IEnumerator Swap() {
		// Wait for the user to observe the scene.
		yield return new WaitForSeconds (transitionTime);

		StartCoroutine (TransitionScene ());
	}

	public void TransitionEarly() {
		StartCoroutine (TransitionScene ());
	}

	IEnumerator TransitionScene() {
		// Wait for the asynchronous load to finish if still incomplete, log progress.
		while (!asyncLoad.isDone) {
			if (Mathf.Approximately (asyncLoad.progress, 0.9f)) { // Scene is ready for activation at 0.9f
				blindPlane.SetActive (true);
				trans.FadeOut ();
				yield return new WaitForSeconds (trans.fadeTime);
				asyncLoad.allowSceneActivation = true;
			}
		}
	}
}
