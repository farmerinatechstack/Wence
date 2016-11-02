using UnityEngine;
using System.Collections;

public class LightFade : MonoBehaviour {
	[SerializeField] Light lt;
	
	// Update is called once per frame
	void Update () {
		lt.intensity = Mathf.PingPong (Time.time, 8);
	}
}
