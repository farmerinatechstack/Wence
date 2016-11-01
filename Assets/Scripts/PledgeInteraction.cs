using UnityEngine;
using System.Collections;

public class PledgeInteraction : MonoBehaviour {
	public bool inGaze;

	[SerializeField] AudioClip tapClip;
	[SerializeField] AudioClip sonarClip;
	[SerializeField] AudioSource src;

	[SerializeField] GameObject root;
	[SerializeField] GameObject pledgeThird;
	[SerializeField] GameObject nextPledge;
	[SerializeField] PlayBubbleEffect bubbles;

	[SerializeField] VRInteractible interactiveItem;

	private IEnumerator fadeAudio;

	private void OnEnable() {
		interactiveItem.OnEnter += HandleEnter;
		interactiveItem.OnExit += HandleExit;

		interactiveItem.OnDown += HandleTap;
	}

	private void OnDisable() {
		interactiveItem.OnEnter -= HandleEnter;
		interactiveItem.OnExit -= HandleExit;

		interactiveItem.OnDown -= HandleTap;
	}

	void Start() {
		src.volume = 0f;
		fadeAudio = FadeAudio (0.1f, 1.0f);
		StartCoroutine (fadeAudio);
	}

	private void HandleEnter() {
		inGaze = true;
		StopCoroutine (fadeAudio);
		fadeAudio = FadeAudio (-0.05f, 0f);
		StartCoroutine (fadeAudio);
	}

	private void HandleExit() {
		inGaze = false;
		StopCoroutine (fadeAudio);
		fadeAudio = FadeAudio (0.05f, 1.0f);
		StartCoroutine (fadeAudio);
	}

	private void HandleTap() {
		if (inGaze)
			StartCoroutine (DisplayCycle() );
	}

	IEnumerator FadeAudio(float step, float target) {
		yield return new WaitForSeconds (0.5f);
		src.loop = true;
		src.clip = sonarClip;
		src.Play();

		while (!Mathf.Approximately(src.volume, target)) {
			src.volume += step;
			yield return new WaitForSeconds (Mathf.Abs(step));
		}
		src.volume = target;
	}

	IEnumerator DisplayCycle() {
		bubbles.PlayBubbles ();
		src.loop = false;
		src.PlayOneShot (tapClip);
		yield return new WaitForSeconds (0.1f);

		pledgeThird.SetActive (true);

		if (nextPledge == null) { // This is the last pledge
			EventManager.instance.TriggerEvent (EventManager.ENVIRONMENT_SET);
		} else {
			nextPledge.SetActive (true);
		}

		Destroy (root, 1.0f);
	}
}
