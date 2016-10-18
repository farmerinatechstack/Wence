using UnityEngine;
using System.Collections;

public class SetFogAndBackground : MonoBehaviour {
	[SerializeField] Color c;
	[SerializeField] float fogDensity;

	// Use this for initialization
	void Awake () {
		RenderSettings.fog = true;
		RenderSettings.fogMode = FogMode.ExponentialSquared;

		Camera.main.backgroundColor = c;
		RenderSettings.fogColor = c;
		RenderSettings.fogDensity = fogDensity;
	}
}
