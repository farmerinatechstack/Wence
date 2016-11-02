using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class AudioFade : MonoBehaviour {
	[SerializeField] AudioSource src;

	private float timeStep = 0.1f;

	void Start () {
		EventManager.instance.StartListening (EventManager.CHANGING_SCENE, FadeAudio);
		src.volume = 0f;
		StartCoroutine(ExecuteFade(40, 0.35f));
	}

	void FadeAudio() {
		if (SceneManager.GetActiveScene().name == "WhenceEnd") {
			StartCoroutine(ExecuteFade(25, 0f));
			Destroy (gameObject, 20 * timeStep); 
		}
	}

	IEnumerator ExecuteFade(int numSteps, float volTarget) {
		float step = volTarget / numSteps;
		step = (src.volume < volTarget) ? step : step * -1f;

		for(int i = 0; i < numSteps; i++) {
			yield return new WaitForSeconds(timeStep);
			src.volume += step;
		}
		src.volume = volTarget;
	}

}
