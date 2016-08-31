using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayPledge : MonoBehaviour {
	[SerializeField] Text pledgeText;
	[SerializeField] Text pledgeTime;
	[SerializeField] Text pledgeCount;



	private float pledgeDelay = 7f;
	private float fadeTime = 2f;

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.ENVIRONMENT_SET, DisplayPledgeInformation);
	}

	private void OnDisable() {
		ExperienceManager.StopListening (ExperienceManager.ENVIRONMENT_SET, DisplayPledgeInformation);
	}

	private void DisplayPledgeInformation() {
		ExperienceManager.StopListening (ExperienceManager.ENVIRONMENT_SET, DisplayPledgeInformation);

		int health = (int) (ExperienceManager.PowerRatio * 100);
		pledgeCount.text = health.ToString () + " %";

		StartCoroutine (Cycle());
	}

	private IEnumerator Cycle() {
		while (true) {
			SMSManager.SMSData d = SMSManager.GetRandomPledgeText ();
			if (d == null) {
				pledgeText.text = "Oh no - we're out of pledges! Send and keep a pledge later so the sea stays beautiful.";
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
