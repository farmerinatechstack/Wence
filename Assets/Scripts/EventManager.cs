using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/* Class: EventManager
 * Singleton used for managing events
 */
public class EventManager : MonoBehaviour {
	public static EventManager instance;

	// Events: the following are string names for all possible events in the experience
	public static string CHANGING_SCENE = "changingScene";			
	public static string ENVIRONMENT_SET = "environmentSet";
	public static string INPUT_DOWN = "inputDown";					// Listeners: SwapScene				
	public static string SEQUENCE_DONE = "sequenceDone";


	private Dictionary<string, UnityEvent> eventDictionary;

	void Awake() {
		if (instance == null) {
			instance = this;
			eventDictionary = new Dictionary<string, UnityEvent> ();
		} else {
			Debug.Log("Destroying GameObject with duplicate EventManager: " + gameObject.name);
			Destroy (gameObject);
		}
	}

	public void StartListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			eventDictionary.Add(eventName, thisEvent);
		}
	}

	public void StopListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public void TriggerEvent(string eventName) {
		UnityEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent)) {
			Debug.Log ("Triggering Event: " + eventName);
			thisEvent.Invoke ();
		}
	}
}
