using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DelayedSwap : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (Swap ());
	}

	IEnumerator Swap() {
		yield return new WaitForSeconds (200f);
		SceneManager.LoadScene ("WenceVirtual");
	}
}
