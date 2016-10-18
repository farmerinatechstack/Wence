using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayPledge : MonoBehaviour {
	[SerializeField] Text pledgeText;
	[SerializeField] Text pledgeTime;
	[SerializeField] Text pledgeCount;

	private float pledgeDelay = 7f;
	private float fadeTime = 2f;

	void Start() {
		pledgeCount.text = SMSManager.Count.ToString ();
		StartCoroutine (Cycle());	
	}

	private IEnumerator Cycle() {
		while (true) {
			SMSManager.SMSData d = SMSManager.GetRandomPledgeText ();
			if (d == null) {
				pledgeText.text = "We're out of pledges! Send and keep a pledge later so Monterey Bay stays beautiful.";
				pledgeTime.text = "Just Now";
			} else {
				pledgeText.text = d.pledge;
				pledgeTime.text = d.time;
			}

			pledgeText.CrossFadeAlpha (1.0f, fadeTime, true);
			pledgeTime.CrossFadeAlpha (1.0f, fadeTime, true);
			yield return new WaitForSeconds (pledgeDelay);

			pledgeText.CrossFadeAlpha (0.0f, fadeTime, true);
			pledgeTime.CrossFadeAlpha (0.0f, fadeTime, true);
			yield return new WaitForSeconds (fadeTime);
		}
	}
}
