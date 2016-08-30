using UnityEngine;
using System.Collections;

public class PledgeDisplay : MonoBehaviour {
	[SerializeField] Animator anim;

	// Is the pledge currently displayed?
	private bool onDisplay = false;

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			TogglePledge ();
		}
	}

	private void TogglePledge() {
		if (!onDisplay) {
			// Get pledge info
			anim.Play ("Show");
		} else {
			anim.Play ("Hide");
		}

		onDisplay = !onDisplay;
	}
}
