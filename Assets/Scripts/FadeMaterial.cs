using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

/* Class: FadeTransition
 * Fades a material to and from transparency */
public class FadeMaterial : MonoBehaviour {
	public float fadeTime = 2.5f;	// Time to fade

	[SerializeField] Material m;
	[SerializeField] MeshRenderer mRenderer;

	private bool fading;
	private IEnumerator fadeRoutine;

	// The material should always start as opaque.
	void Awake () {
		m.color = new Color (m.color.r, m.color.g, m.color.b, 1.0f);
		mRenderer.enabled = true;
		m.renderQueue = 4000; // The overlay level.

		fading = false;
	}

	void OnDestroy() {
		m.color = new Color (m.color.r, m.color.g, m.color.b, 0.0f);
		mRenderer.enabled = true;
	}

	/* Function: FadeIn
	 * Fade from color to transparent */
	public void FadeIn() {
		if (fading) StopCoroutine (fadeRoutine);
		fadeRoutine = FadeTo (0.0f, fadeTime);

		StartCoroutine (fadeRoutine);
	}

	/* Function: FadeTo
	 * Fade to black from transparent */
	public void FadeOut() {
		if (fading) StopCoroutine (fadeRoutine);
		fadeRoutine = FadeTo (1.0f, fadeTime);

		StartCoroutine (fadeRoutine);
	}

	IEnumerator FadeTo(float targetAlpha, float time) {
		fading = true;
		mRenderer.enabled = true;
		float startAlpha = m.color.a;

		for (float t = 0.0f; t <= time; t += Time.deltaTime) {
			float fadeAlpha = Mathf.Lerp (startAlpha, targetAlpha, t / time);
			m.color = new Color(m.color.r, m.color.g, m.color.b, fadeAlpha);
			yield return null; 		// Wait for one frame
		} 

		if (Mathf.Approximately(0.0f, targetAlpha)) mRenderer.enabled = false;
		m.color = new Color(m.color.r, m.color.g, m.color.b, targetAlpha);
		fading = false;
	}

	void HideRenderer() {
		mRenderer.enabled = false;
	}
}
