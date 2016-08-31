using UnityEngine;
using System.Collections;

public class ToggleBubbles : MonoBehaviour {
	[SerializeField] GameObject playerBubbles;

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.ENVIRONMENT_SET, Toggle);
	}

	private void OnDisable() {
		ExperienceManager.StopListening (ExperienceManager.ENVIRONMENT_SET, Toggle);
	}

	private void Toggle() {
		playerBubbles.SetActive(true);
	}
}
