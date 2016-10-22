using UnityEngine;
using System.Collections;

public class WaitToStart : MonoBehaviour {
	public float waitTime;

	[SerializeField] GameObject[] objs;

	// Use this for initialization
	void Start () {
		StartCoroutine (WaitToEnable ());
	}

	IEnumerator WaitToEnable() {
		yield return new WaitForSeconds (waitTime);

		foreach (GameObject o in objs) {
			o.SetActive (true);
		}
	}
}
