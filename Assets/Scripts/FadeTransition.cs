using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

/* Class: FadeTransition
 * Fades a material on an object to and from transparency */
public class FadeTransition : MonoBehaviour {
	public Material m;
	public float fadeTime = 2.5f;	// Time to fade

	// The material should always start as opaque, and fade in.
	void Start () {
		m.color = new Color (m.color.r, m.color.g, m.color.b, 1.0f);
		m.renderQueue = 4000; // The overlay level.
	}

	void OnDestroy() {
		m.color = new Color (m.color.r, m.color.g, m.color.b, 1.0f);
	}

	/* Function: FadeIn
	 * Fade from color to transparent */
	public void FadeIn() {
		StartCoroutine (FadeTo (0.0f, fadeTime));
	}

	/* Function: FadeTo
	 * Fade to black from transparent */
	public void FadeOut() {
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
