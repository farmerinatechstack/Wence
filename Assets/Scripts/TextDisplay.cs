using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextDisplay : MonoBehaviour {
	[SerializeField] float delay;
	[SerializeField] bool fadeText;
	[SerializeField] Text text;

	private float fadeLength = 1f;

	// Use this for initialization
	void Start () {
		text.CrossFadeAlpha (0f, 0f, true);
		StartCoroutine (FadeCycle ());
	}

	IEnumerator FadeCycle() {
		yield return new WaitForSeconds (delay);
		text.CrossFadeAlpha (1.0f, fadeLength, true);

		while (fadeText) {
			yield return new WaitForSeconds (2f);
			text.CrossFadeAlpha (0f, 1.5f, true);
			yield return new WaitForSeconds (2f);
			text.CrossFadeAlpha (1.0f, 1.5f, true);
		}
	}
}
