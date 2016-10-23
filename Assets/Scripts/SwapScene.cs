using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* Class: SwapScene
 * Swaps to sceneName on event or after a set time. Uses a fade
 * transition if provided.
 */
public class SwapScene : MonoBehaviour {
	[SerializeField] AudioSource src;
	[SerializeField] bool transitionByEvent;
	[SerializeField] FadeMaterial transitionFader;
	[SerializeField] float transitionTime;
	[SerializeField] string sceneName;

	AsyncOperation asyncLoad;

	private void OnDisable() {
		EventManager.instance.StopListening (EventManager.INPUT_DOWN, HandleSwap);
	}

	private void Start() {
		EventManager.instance.StartListening (EventManager.INPUT_DOWN, HandleSwap);

		if (!string.IsNullOrEmpty(sceneName)) {
			asyncLoad = SceneManager.LoadSceneAsync(sceneName);
			asyncLoad.allowSceneActivation = false;
		} else {
			Debug.LogError ("No transition scene detected");
		}

		if (transitionFader) {
			transitionFader.FadeIn ();
		} else {
			Debug.LogError ("No transition fader detected");
		}
		
		if (!transitionByEvent)
			StartCoroutine (WaitToTransition ());
	}

	void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(Swap());
		}
		#endif
	}

	private void HandleSwap() {
		if (transitionByEvent) {
			VRInput.instance.enableInput = false;
			src.Play ();
			StartCoroutine (Swap ());
		}
	}

	IEnumerator WaitToTransition() {
		yield return new WaitForSeconds (transitionTime);
		StartCoroutine (Swap ());
	}

	IEnumerator Swap() {

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