using UnityEngine;
using System.Collections;

public class SplitAudioHandler : MonoBehaviour {
	[SerializeField] AudioSource src;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		StartCoroutine (HandleAudio ());
	}

	IEnumerator HandleAudio() {
		src.Play ();

		yield return new WaitForSeconds (200f);
		Destroy (gameObject);
	}
}
