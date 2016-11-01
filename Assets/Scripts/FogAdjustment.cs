using UnityEngine;
using System.Collections;

public class FogAdjustment : MonoBehaviour {
	[SerializeField] Color downColor;
	[SerializeField] Color upColor;

	private int numTimeSteps = 100;
	private float secondsPerStep = 0.08f;

	// Use this for initialization
	void Awake () {
		RenderSettings.fog = true;
		Camera.main.backgroundColor = downColor;
		RenderSettings.fogColor = downColor;
	}

	void Start() {
		StartCoroutine (ShiftEnvironment ());
	}

	/* Function: ShiftEnvironment
	 * Shifts the fog and environment color from the lowest level to the current based on power ratio. */
	private IEnumerator ShiftEnvironment() {
		//Color finalColor = Color.Lerp (downColor, upColor, SMSManager.instance.PowerRatio);
		Color finalColor = upColor;
		float startDensity = RenderSettings.fogDensity;
		//float finalDensity = RenderSettings.fogDensity - (0.5f) / 5f;
		float finalDensity = 0.2f;
		yield return new WaitForSeconds (4.0f);

		for (int timeStep = 1; timeStep <= numTimeSteps; timeStep++) {
			Color currColor = Color.Lerp (downColor, finalColor, (1.0f * timeStep / numTimeSteps));
			float currDensity = Mathf.Lerp (startDensity, finalDensity, (1.0f * timeStep / numTimeSteps));

			RenderSettings.fogDensity = currDensity;
			RenderSettings.fogColor = currColor;
			Camera.main.backgroundColor = currColor;
			yield return new WaitForSeconds (secondsPerStep);
		}
	}
}
