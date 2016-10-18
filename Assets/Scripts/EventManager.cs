using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/* Class: EventManager
 * Singleton used for managing events
 */
public class EventManager : MonoBehaviour {
	public static bool experienceReady = false;

	// Events: the following are string names for all possible events in the experience
	public static string ENVIRONMENT_SET = "environmentSet";
	public static string WAITING_FOR_PLEDGES = "waitingForPledges";
	public static string WAITING_FOR_SHIFT = "waitingForShift";

	private static EventManager instance;
	private static Dictionary<string, UnityEvent> eventDictionary;

	void Awake() {
		if (!instance) {
			instance = this;
			eventDictionary = new Dictionary<string, UnityEvent> ();
		} else {
			Debug.LogError ("Destroying gameObject with duplicate ExperienceManager instance: " + gameObject.name);
			Destroy (gameObject);
		}
	}

	public static void StartListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName) {
		UnityEvent thisEvent = null;
		if (eventDictionary.TryGetValue (eventName, out thisEvent)) {
			Debug.Log ("Triggering Event: " + eventName);
			thisEvent.Invoke ();
		}
	}
}
