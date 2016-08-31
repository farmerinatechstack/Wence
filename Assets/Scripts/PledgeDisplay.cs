using UnityEngine;
using System.Collections;

public class PledgeDisplay : MonoBehaviour {
	[SerializeField] float displayTime;
	[SerializeField] Animator anim;

	// Is the pledge currently displayed?
	private bool onDisplay = false;

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.PLEDGE_SELECTED, TogglePledge);
	}

	private void OnDisable() {
		ExperienceManager.StartListening (ExperienceManager.PLEDGE_SELECTED, TogglePledge);
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			TogglePledge ();
		}
	}


	private void TogglePledge() {
		if (!onDisplay) {
			// TODO: Get pledge info

			anim.Play ("Show");
			StartCoroutine (DelayedHide());
		} else {
			anim.Play ("Hide");
		}

		onDisplay = !onDisplay;
	}

	/* Function: DelayedHide
	 * Waits to hide the pledge. */
	private IEnumerator DelayedHide() {
		yield return new WaitForSeconds (displayTime);

		TogglePledge ();

		// Wait for the animation to complete
		yield return new WaitForSeconds(2f);
		ExperienceManager.TriggerEvent (ExperienceManager.PLEDGE_HIDDEN);
	}
}
