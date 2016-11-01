using UnityEngine;
using System.Collections;

public class PlayBubbleEffect : MonoBehaviour {
	[SerializeField] AudioSource src;
	[SerializeField] ParticleSystem bubbles;

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
