using UnityEngine;
using System.Collections;

public class TransformBob : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		float yBob = Mathf.Sin (Time.realtimeSinceStartup) * 0.001f;
		transform.position += new Vector3(0, yBob, 0);
	}
}
