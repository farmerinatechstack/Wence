using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

/* Class: GeneralFader
 * Fades any material on an object to and from transparency */
public class GeneralFader : MonoBehaviour {
	public Material m;
	public float fadeTime = 2.5f;	// Time to fade
	public string sceneName;		// Name of scene to transition to

	// The material should always start as opaque.
	void Start () {
		m.color = new Color (m.color.r, m.color.g, m.color.b, 1.0f);
		FadeFrom ();
	}

	void OnDestroy() {
		m.color = new Color (m.color.r, m.color.g, m.color.b, 1.0f);
	}

	/* Function: FadeFrom
	 * Fade from color to transparent */
	public void FadeFrom() {
		StartCoroutine (FadeTo (0.0f, fadeTime));
	}

	/* Function: FadeTo
	 * Fade to transparent from color */
	public void FadeTo() {
		StartCoroutine (FadeTo(1.0f, fadeTime));
	}

	IEnumerator FadeTo(float targetAlpha, float time) {
		float startAlpha = m.color.a;

		for (float t = 0.0f; t <= time; t += Time.deltaTime) {
			float fadeAlpha = Mathf.Lerp (startAlpha, targetAlpha, t / time);
			Color newColor = new Color(m.color.r, m.color.g, m.color.b, fadeAlpha);
			m.color = newColor;
			yield return null; 		// Wait for one frame
		}

		Color finalColor = new Color(m.color.r, m.color.g, m.color.b, targetAlpha);
		m.color = finalColor;		// Set the alpha to the target alpha
	}
}
