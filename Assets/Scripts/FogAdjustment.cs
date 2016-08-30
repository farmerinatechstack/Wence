using UnityEngine;
using System.Collections;

public class FogAdjustment : MonoBehaviour {
	[SerializeField] Color downColor;
	[SerializeField] Color upColor;

	private Material backgroundMaterial;

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.INFO_LOADED, SetColor);
	}

	private void OnDisable() {
		ExperienceManager.StopListening (ExperienceManager.INFO_LOADED, SetColor);
	}

	// Use this for initialization
	void Start () {
		backgroundMaterial = gameObject.GetComponent<MeshRenderer> ().material;
	}

	/* Function: SetColor
	 * Sets the color of the fog and background sphere material based on PledgePower. */
	private void SetColor() {
		Color powerColor = Color.Lerp (downColor, upColor, ExperienceManager.PowerRatio);

		backgroundMaterial.color = powerColor;
		RenderSettings.fogColor = powerColor;
	}
}
