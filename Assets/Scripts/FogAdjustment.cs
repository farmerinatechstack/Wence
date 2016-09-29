﻿using UnityEngine;
using System.Collections;

public class FogAdjustment : MonoBehaviour {
	[SerializeField] Color downColor;
	[SerializeField] Color upColor;
	[SerializeField] Camera cam;

	private int numTimeSteps = 100;
	private float secondsPerStep = 0.08f;

	// Use this for initialization
	void Start () {
		cam.backgroundColor = downColor;
		RenderSettings.fogColor = downColor;
		StartCoroutine (WaitForReady ());
	}

	private void Update() {
		if (Input.GetKeyDown (KeyCode.G)) {
			StartCoroutine(ShiftEnvironment());
		}
	}

	IEnumerator WaitForReady() {
		while (!ExperienceManager.experienceReady) {
			yield return new WaitForSeconds (1.0f);
		}
		StartShift ();
	}


	void StartShift() {
		StartCoroutine (ShiftEnvironment ());
	}

	/* Function: ShiftEnvironment
	 * Shifts the fog and environment color from the lowest level to the current based on power ratio. */
	private IEnumerator ShiftEnvironment() {
		Color finalColor = Color.Lerp (downColor, upColor, ExperienceManager.PowerRatio);
		float startDensity = RenderSettings.fogDensity;
		float finalDensity = RenderSettings.fogDensity - (ExperienceManager.PowerRatio) / 10;
		yield return new WaitForSeconds (4.0f);

		for (int timeStep = 1; timeStep <= numTimeSteps; timeStep++) {
			Color currColor = Color.Lerp (downColor, finalColor, (1.0f * timeStep / numTimeSteps));
			float currDensity = Mathf.Lerp (startDensity, finalDensity, (1.0f * timeStep / numTimeSteps));

			RenderSettings.fogDensity = currDensity;
			RenderSettings.fogColor = currColor;
			cam.backgroundColor = currColor;
			yield return new WaitForSeconds (secondsPerStep);
		}
	}
}
