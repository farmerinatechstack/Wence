using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* Class: SwapScene
 * Swaps to sceneName on event or after a set time. Uses a fade
 * transition if provided.
 */
public class SwapScene : MonoBehaviour {
	[SerializeField] FadeMaterial transitionFader;
	[SerializeField] string sceneName;
	[SerializeField] bool transitionByEvent;
	[SerializeField] float transitionTime;

	AsyncOperation asyncLoad;

	private void OnEnable() {
		VRInput.OnInputDown += HandleSwap;
	}

	private void OnDisable() {
		VRInput.OnInputDown -= HandleSwap;
	}

	private void Start() {
		if (!string.IsNullOrEmpty(sceneName)) {
			asyncLoad = SceneManager.LoadSceneAsync(sceneName);
			asyncLoad.allowSceneActivation = false;
		} else {
			Debug.LogError ("No transition scene detected");
		}

		if (transitionFader) {
			transitionFader.FadeIn ();
		} else {
			Debug.LogWarning ("No transition fader detected");
		}
		
		if (!transitionByEvent)
			StartCoroutine (WaitToTransition ());
	}

	void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.Space)) {
			ExecuteSwap();
		}
		#endif
	}

	private void HandleSwap() {
		if (transitionByEvent) {
			ExecuteSwap ();
		}
	}

	public void ExecuteSwap() {
		VRInput.enableInput = false;
		StartCoroutine (Swap ());
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
				if (transitionFader) {
					transitionFader.FadeOut ();
					yield return new WaitForSeconds (transitionFader.fadeTime);
				}
				asyncLoad.allowSceneActivation = true;
			} else {
				Debug.LogWarning ("Still loading to swap scene...");
				yield return new WaitForSeconds (1.0f);
			}
		}
	}
}