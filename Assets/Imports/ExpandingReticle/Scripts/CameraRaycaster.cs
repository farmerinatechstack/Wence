using UnityEngine;
using System;

// Executes raycasts and exposes GameObjects with VRInteractiveItem
public class CameraRaycaster: MonoBehaviour {
	[SerializeField] private Transform vrCamera;
	[SerializeField] private ExpandingReticle reticle;
	[SerializeField] private bool showDebugRay;               	// Optionally show the debug ray
	[SerializeField] private float debugRayDuration = 1f;       // How long the Debug ray will remain visible
	[SerializeField] private float rayLength = 10f;      		// How far into the scene the ray is cast
	[SerializeField] private LayerMask m_ExclusionLayers;    	// Layers to exclude from the raycast

	private VRInteractible currentInteractible;
	private VRInteractible lastInteractible;

	// Expose the interactive item
	public VRInteractible CurrentInteractible {
		get { return currentInteractible; }
	}

	// Update is called once per frame
	void Update () {
		EyeRaycast ();
	}

	private void EyeRaycast() {
		if (showDebugRay) {
			Debug.DrawRay (vrCamera.position, vrCamera.forward * rayLength, Color.blue, debugRayDuration);
		}

		// Execute raycast
		Ray ray = new Ray(vrCamera.position, vrCamera.forward);
		RaycastHit hit;
		if (Physics.Raycast (ray, out hit, rayLength, ~m_ExclusionLayers)) {
			currentInteractible = hit.collider.GetComponent<VRInteractible> ();
			if (currentInteractible == null)
				return;

			if (currentInteractible != lastInteractible) {
				currentInteractible.Enter ();
				DeactivateLastInteractible ();
			}

			lastInteractible = currentInteractible;
			reticle.SetGazeTarget (hit.point, true);
		} else {
			DeactivateLastInteractible ();
			currentInteractible = null;
			reticle.SetGazeTarget (vrCamera.position + (vrCamera.forward * 10f), false);
		}
	}

	private void DeactivateLastInteractible() {
		if (lastInteractible == null)
			return;

		lastInteractible.Exit ();
		lastInteractible = null;
	}
}
