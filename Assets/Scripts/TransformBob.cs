using UnityEngine;
using System.Collections;

public class TransformBob : MonoBehaviour {
	public bool randomize;
	public float offsetFactor = 0.001f;

	void Start() {
		if (randomize)
			offsetFactor = Random.Range (0.001f, 0.005f);
	}

	// Update is called once per frame
	void Update () {
		float yBob = Mathf.Sin (Time.realtimeSinceStartup) * offsetFactor;
		transform.position += new Vector3(0, yBob, 0);
	}
}
