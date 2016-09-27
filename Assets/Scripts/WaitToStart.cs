using UnityEngine;
using System.Collections;

public class WaitToStart : MonoBehaviour {
	public float waitTime;

	[SerializeField] GameObject bubblesLeft;
	[SerializeField] GameObject bubblesRight;
	[SerializeField] AudioSource scubaSound;

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitToEnable ());
	}

	IEnumerator WaitToEnable() {
		yield return new WaitForSeconds (waitTime);
		bubblesLeft.SetActive (true);
		bubblesRight.SetActive (true);
		scubaSound.enabled = true;
	}
}
