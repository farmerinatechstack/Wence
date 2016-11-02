using UnityEngine;
using System.Collections;

public class PlayBubbleEffect : MonoBehaviour {
	[SerializeField] AudioSource src;
	[SerializeField] ParticleSystem bubbles;

	void Update() {
		#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.P)) {
			bubbles.Play ();
		}
		#endif
	}

	public void PlayBubbles() {
		VRInput.instance.enableInput = false;
		src.Play ();
		bubbles.Play ();

		Invoke ("EnableInput", bubbles.startLifetime);
	}

	void EnableInput() {
		VRInput.instance.enableInput = true;
	}
}
