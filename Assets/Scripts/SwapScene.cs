using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* Class: SwapScene
 * Swaps to sceneName on event or after a set time. Uses a fade
 * transition if provided.
 */
public class SwapScene : MonoBehaviour {
	[SerializeField] AudioSource src;

	// Three transition types exist: timed, user input, or sequence. A sequence
	// transition enables a timed or user input transition after a sequence.
	[SerializeField] bool transitionAfterTime;
	[SerializeField] bool transitionAfterEvent;
	[SerializeField] bool sequenceDone;

	[SerializeField] FadeMaterial transitionFader;
	[SerializeField] float transitionTime;
	[SerializeField] string sceneName;

	AsyncOperation asyncLoad;

	private void Start() {
		if (!string.IsNullOrEmpty(sceneName)) {
			asyncLoad = SceneManager.LoadSceneAsync(sceneName);
			asyncLoad.allowSceneActivation = false;
		} else {
			Debug.LogError ("No transition scene detected");
		}
		if (transitionFader) transitionFader.FadeIn ();

		EventManager.instance.StartListening (EventManager.SEQUENCE_DONE, ToggleSequence);
		EventManager.instance.StartListening (EventManager.INPUT_DOWN, HandleSwap);
		if (transitionAfterTime && sequenceDone)
			StartCoroutine (WaitToTransition ());
	}

	void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(Swap());
		}
		#endif
	}

	IEnumerator WaitToTransition() {
		yield return new WaitForSeconds (transitionTime);
		StartCoroutine (Swap ());
	}

	private void ToggleSequence() {
		sequenceDone = true;
		if (transitionAfterTime)
			StartCoroutine (WaitToTransition ()); 
	}

	private void HandleSwap() {
		if (transitionAfterEvent && sequenceDone) {
			src.Play ();
			StartCoroutine (Swap ());
		}
	}

	IEnumerator Swap() {
		VRInput.instance.enableInput = false;
		while (!asyncLoad.isDone) {
			float progress = asyncLoad.progress / 0.9f;
			yield return null;

			if (Mathf.Approximately (asyncLoad.progress, 0.9f)) {
				EventManager.instance.TriggerEvent (EventManager.CHANGING_SCENE);
				if (transitionFader) {
					transitionFader.FadeOut ();
					yield return new WaitForSeconds (transitionFader.fadeTime);
				}

				VRInput.instance.enableInput = true;
				asyncLoad.allowSceneActivation = true;
			} else {
				Debug.LogWarning ("Still loading to swap scene...");
				yield return new WaitForSeconds (1.0f);
			}
		}
	}
}