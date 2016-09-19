using UnityEngine;
using System.Collections;

public class FogAdjustment : MonoBehaviour {
	[SerializeField] Color downColor;
	[SerializeField] Color upColor;
	[SerializeField] Material backgroundMaterial;

	private Color originalMaterialColor;
	private int numTimeSteps = 50;
	private float secondsPerStep = 0.03f;

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
		originalMaterialColor = backgroundMaterial.color;

		backgroundMaterial.color = downColor;
		RenderSettings.fogColor = downColor;
	}

	void OnDestroy() {
		backgroundMaterial.color = originalMaterialColor;
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

		for (int timeStep = 1; timeStep <= numTimeSteps; timeStep++) {
			Color currColor = Color.Lerp (downColor, finalColor, (1.0f * timeStep / numTimeSteps));
			float currDensity = Mathf.Lerp (startDensity, finalDensity, (1.0f * timeStep / numTimeSteps));


			RenderSettings.fogDensity = currDensity;
			RenderSettings.fogColor = currColor;
			backgroundMaterial.color = currColor;
			Debug.Log ("Taking fog step. Density " + currDensity);
			if (timeStep > numTimeSteps / 3) { 	// Slow the transition after the initial speedy transition.
				yield return new WaitForSeconds (secondsPerStep * 20f);
			} else {							// Start with a fast transition
				yield return new WaitForSeconds (secondsPerStep);
			}
		}
	}
}
