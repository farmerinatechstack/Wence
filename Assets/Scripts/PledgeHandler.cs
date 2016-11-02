using UnityEngine;
using System.Collections;

/* Class: PledgeHandler
 * Waits to start the pledge sequence, and triggers full environment on completion of
 * pledge sequence.
 */
public class PledgeHandler : MonoBehaviour {
	public float waitTime = 5f;

	[SerializeField] GameObject bakedFloor;
	[SerializeField] GameObject unbakedFloor;

	[SerializeField] FadeMaterial fader;
	[SerializeField] GameObject[] toEnable;
	[SerializeField] GameObject firstPledge;

	private void OnDisable() {
		EventManager.instance.StopListening (EventManager.ENVIRONMENT_SET, CompleteEnvironment);
	}

	// Use this for initialization
	void Start () {
		EventManager.instance.StartListening (EventManager.ENVIRONMENT_SET, CompleteEnvironment);
		VRInput.instance.enableInput = false;

		StartCoroutine (WaitToDisplayPledge ());
	}

	IEnumerator WaitToDisplayPledge() {
		yield return new WaitForSeconds (waitTime);

		fader.fadeTime = 1.0f;
		fader.FadeOut ();
		yield return new WaitForSeconds (fader.fadeTime);
		VRInput.instance.enableInput = true;
		firstPledge.SetActive (true);
		fader.FadeIn ();
	}

	private void CompleteEnvironment() {
		// Swap the floors used
		unbakedFloor.SetActive(false);
		bakedFloor.SetActive (true);

		foreach (GameObject obj in toEnable) {
			obj.SetActive (true);
		}
		EventManager.instance.TriggerEvent (EventManager.SEQUENCE_DONE);
	}
}
