using UnityEngine;
using System.Collections;

/* Class: PledgeHandler
 * Waits to start the pledge sequence, and triggers full environment on completion of
 * pledge sequence.
 */
public class PledgeHandler : MonoBehaviour {
	public float waitTime = 5f;

	[SerializeField] PlayBubbleEffect bubbles;
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
		bubbles.PlayBubbles ();
		VRInput.instance.enableInput = true;

		yield return new WaitForSeconds (0.5f);
		firstPledge.SetActive (true);
	}

	private void CompleteEnvironment() {
		foreach (GameObject obj in toEnable) {
			obj.SetActive (true);
		}
		EventManager.instance.TriggerEvent (EventManager.SEQUENCE_DONE);
	}
}
