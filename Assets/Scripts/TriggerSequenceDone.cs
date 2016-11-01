using UnityEngine;
using System.Collections;

public class TriggerSequenceDone : MonoBehaviour {
	[SerializeField] float sequenceTime;

	// Use this for initialization
	void Start () {
		Invoke ("TriggerSequenceEvent", sequenceTime);
	}
	
	public void TriggerSequenceEvent() {
		EventManager.instance.TriggerEvent(EventManager.SEQUENCE_DONE);
	}
}
