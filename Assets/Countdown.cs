using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour {
	[SerializeField] int countdownTime = 60;
	[SerializeField] Text countdownText;

	// Use this for initialization
	void Start () {
		StartCoroutine (startCountdown ());	
	}

	IEnumerator startCountdown() {
		while (countdownTime >= 0) {
			countdownText.text = countdownTime.ToString();
			yield return new WaitForSeconds (1.0f);
			countdownTime--;
		}
	}
}
