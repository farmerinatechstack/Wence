using UnityEngine;
using System.Collections;

public class WaitForExperienceReady : MonoBehaviour {
	[SerializeField] WaitToStart bubbleDelay;
	[SerializeField] ManageSceneView blindManager;

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitForReady ());
	}

	IEnumerator WaitForReady() {
		while (!ExperienceManager.experienceReady) {
			yield return new WaitForSeconds (1.0f);
		}

		bubbleDelay.enabled = true;
		blindManager.enabled = true;
		ExperienceManager.TriggerEvent (ExperienceManager.ENVIRONMENT_SET);
	}
}
