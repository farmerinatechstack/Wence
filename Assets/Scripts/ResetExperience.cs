using UnityEngine;
using System.Collections;

public class ResetExperience : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject[] restarts = GameObject.FindGameObjectsWithTag ("DestroyOnRestart");

		foreach (GameObject obj in restarts) {
			Destroy(obj);
		}
	}
}
