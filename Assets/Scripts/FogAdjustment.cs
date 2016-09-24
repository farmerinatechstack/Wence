using UnityEngine;
using System.Collections;

public class FogAdjustment : MonoBehaviour {
	[SerializeField] Color downColor;
	[SerializeField] Color upColor;
	[SerializeField] Camera cam;

	private int numTimeSteps = 100;
	private float secondsPerStep = 0.05f;

	private void Update() {
		if (Input.GetKeyDown (KeyCode.G)) {
			StartCoroutine(ShiftEnvironment());
		}
	}

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.INFO_LOADED, StartShift);
	}

	private void OnDisable() {
		ExperienceManager.StopListening (ExperienceManager.INFO_LOADED, StartShift);
	}

	// Use this for initialization
	void Start () {
		cam.backgroundColor = downColor;
		RenderSettings.fogColor = downColor;
	}

	void StartShift() {
		StartCoroutine (ShiftEnvironment ());
	}

	/* Function: ShiftEnvironment
	 * Shifts the fog and environment color from the lowest level to the current based on power ratio. */
	private IEnumerator ShiftEnvironment() {
		Color finalColor = Color.Lerp (downColor, upColor, ExperienceManager.PowerRatio);
		//Color finalColor = upColor;
		float startDensity = RenderSettings.fogDensity;
		float finalDensity = RenderSettings.fogDensity - (ExperienceManager.PowerRatio) / 10;
		//float finalDensity = 0.2f;

		for (int timeStep = 1; timeStep <= numTimeSteps; timeStep++) {
			Color currColor = Color.Lerp (downColor, finalColor, (1.0f * timeStep / numTimeSteps));
			float currDensity = Mathf.Lerp (startDensity, finalDensity, (1.0f * timeStep / numTimeSteps));


			RenderSettings.fogDensity = currDensity;
			RenderSettings.fogColor = currColor;
			cam.backgroundColor = currColor;
			Debug.Log ("Taking fog step. Density " + currDensity);
			if (timeStep > numTimeSteps / 2) { 	// Slow the transition after the initial speedy transition.
				yield return new WaitForSeconds (secondsPerStep * 5f);
			} else {							// Start with a fast transition
				yield return new WaitForSeconds (secondsPerStep);
			}
		}
	}
}
