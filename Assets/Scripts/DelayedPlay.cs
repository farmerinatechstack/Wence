using UnityEngine;
using System.Collections;

public class DelayedPlay : MonoBehaviour {
	[SerializeField] AudioSource src;
	[SerializeField] float delayTime;

	// Use this for initialization
	void Start () {
		Invoke ("PlayAudio", delayTime);
	}

	void PlayAudio() {
		src.Play ();
	}
}
