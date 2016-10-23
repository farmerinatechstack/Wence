using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioFade : MonoBehaviour {
	[SerializeField] AudioSource src;

	void Start () {
		EventManager.instance.StartListening (EventManager.CHANGING_SCENE, FadeAudio);
	}

	void FadeAudio() {
		if (SceneManager.GetActiveScene().name == "WhenceDynamic") {
			StartCoroutine(ExecuteFade());
		}
	}

	IEnumerator ExecuteFade() {
		float timeElapsed = 0f;

		for(int i = 0; i < 20; i++) {
			yield return new WaitForSeconds(0.1f);

			src.volume -= 0.025f;
		}
		src.volume = 0f;
	}

}
