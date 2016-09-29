using UnityEngine;
using System.Collections;

public class DelayedPlay : MonoBehaviour {
	[SerializeField] AudioSource audio;
	[SerializeField] float delayTime;


	// Use this for initialization
	void Start () {
		Invoke ("PlayAudio", delayTime);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void PlayAudio() {
		audio.Play ();
	}
}
