using UnityEngine;
using System.Collections;

/* Class: PowerItem
 * Handles the properties of a PowerItem spawned to the Sea Floor */
public class PowerItem : MonoBehaviour {
	public bool randomizeRotation;
	[SerializeField] private float maxRotX;
	[SerializeField] private float maxRotY;
	[SerializeField] private float maxRotZ;

	public bool randomizeScale;
	[SerializeField] private float scaleMin;
	[SerializeField] private float scaleMax;

	[SerializeField] private float yOffset;
	[SerializeField] private float objRadius;

	//[SerializeField] private VRAssets.ReticleRadial radial;
	//[SerializeField] private bool inGaze;
	//[SerializeField] private bool canBeSelected;
	//[SerializeField] private VRAssets.VRInteractiveItem interactiveItem;

	private void Awake() {
		/*
		canBeSelected = true;
		radial = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<VRAssets.ReticleRadial> ();

		if (randomizeScale) {
			float scale = Random.Range (scaleMin, scaleMax);
			transform.localScale = new Vector3 (scale, scale, scale);
		}
		*/
	}

	/*
	private void OnEnable() {
		interactiveItem.OnEnter += HandleEnter;
		interactiveItem.OnExit += HandleExit;
		radial.OnSelectionComplete += HandleSelected;

		ExperienceManager.StartListening (ExperienceManager.PLEDGE_SELECTED, ToggleCanBeSelected);
		ExperienceManager.StartListening (ExperienceManager.PLEDGE_HIDDEN, ToggleCanBeSelected);
	}

	private void OnDisable() {
		interactiveItem.OnEnter -= HandleEnter;
		interactiveItem.OnExit -= HandleExit;
		radial.OnSelectionComplete -= HandleSelected;

		ExperienceManager.StopListening (ExperienceManager.PLEDGE_SELECTED, ToggleCanBeSelected);
		ExperienceManager.StopListening (ExperienceManager.PLEDGE_HIDDEN, ToggleCanBeSelected);
	}

	private void HandleEnter() {
		if (canBeSelected) {
			radial.Show ();
			inGaze = true;
		}
	}

	private void HandleExit() {
		if (canBeSelected) {
			radial.Hide ();
			inGaze = false;
		}
	}

	private void HandleSelected() {
		if (inGaze && canBeSelected) {
			ExperienceManager.TriggerEvent (ExperienceManager.PLEDGE_SELECTED);

			inGaze = false;
			radial.Hide ();
		}
	}

	private void ToggleCanBeSelected() {
		canBeSelected = !canBeSelected;
	}
	*/

	public Quaternion getRotation() {
		if (!randomizeRotation) return transform.rotation;

		return Quaternion.Euler (new Vector3 (
			Random.Range (0f, maxRotX), 
			Random.Range (0f, maxRotY), 
			Random.Range (0f, maxRotZ)));
	}

	public float getYOffset() {
		return yOffset;
	}

	public float getObjRadius() {
		return objRadius;
	}
}
