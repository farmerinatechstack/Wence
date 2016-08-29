using UnityEngine;
using System.Collections;

/* Class: PowerItem
 * Handles the properties of a PowerItem spawned to the Sea Floor */
public class PowerItem : MonoBehaviour {
	[SerializeField] private bool randomizeRotation;
	[SerializeField] private bool randomizeScale;
	[SerializeField] private float scaleMin;
	[SerializeField] private float scaleMax;

	[SerializeField] private float yOffset;
	[SerializeField] private float objRadius;

	// Configure the appearance of the object in the scene.
	private void Start() {
		if (randomizeScale) {
			transform.localScale = new Vector3 (
				Random.Range (scaleMin, scaleMax),
				Random.Range (scaleMin, scaleMax),
				Random.Range (scaleMin, scaleMax));
		}
	}

	public Quaternion getRotation() {
		if (!randomizeRotation) return transform.rotation;

		float maxRotation = 90f;

		return Quaternion.Euler (new Vector3 (
			Random.Range (0f, maxRotation), 
			Random.Range (0f, maxRotation), 
			Random.Range (0f, maxRotation)));
	}

	public float getYOffset() {
		return yOffset;
	}

	public float getObjRadius() {
		return objRadius;
	}
}
