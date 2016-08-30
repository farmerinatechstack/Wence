using UnityEngine;
using System.Collections;

/* Class: MoveForward
 * Apply to a GameObject which should move forward at a constant rate. */
public class MoveForward : MonoBehaviour {
	[SerializeField] float forwardRateMin;
	[SerializeField] float forwardRateMax;
	
	// Update is called once per frame
	void Update () {
		transform.position = transform.position - (transform.forward * Random.Range(forwardRateMin, forwardRateMax) * Time.deltaTime);
	}
}
