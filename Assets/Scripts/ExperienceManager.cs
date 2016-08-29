using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

/* Class: ExperienceManager
 * Singleton used for managing experience-related events
 */
public class ExperienceManager : MonoBehaviour {
	public static int Power;
	public static int MAX_POWER = 1000;


	// Events: the following are string names for all possible events in the experience
	public static string INFO_LOADED = "infoLoaded";
	public static string ENVIRONMENT_SET = "environmentSet";

	public static ExperienceManager instance {
		get {
			if (!eventManager) {
				eventManager = FindObjectOfType (typeof(ExperienceManager)) as ExperienceManager;
				eventManager.Init ();
			}
			return eventManager;
		}
	}

	private static ExperienceManager eventManager;
	private Dictionary<string, UnityEvent> eventDictionary;

	private void Start() {
		Power = GetPower ();
		TriggerEvent (INFO_LOADED);
	}

	/* Function: Update
	 * Used to mock events for testing */
	private void Update() {
		if (Input.GetKeyDown(KeyCode.Space)) {
			TriggerEvent (INFO_LOADED);
		}
	}

	private void Init() {
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent> ();
		}
	}

	/* Function: GetPower
	 * Returns the power of the environment */
	private int GetPower() {
		// TODO: read from website to get environment power
		return 500;
	}

	public static void StartListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.AddListener(listener);
		} else {
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			instance.eventDictionary.Add(eventName, thisEvent);
		}
	}

	public static void StopListening(string eventName, UnityAction listener) {
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue(eventName, out thisEvent)) {
			thisEvent.RemoveListener(listener);
		}
	}

	public static void TriggerEvent(string eventName) {
		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.Invoke ();
		}
	}
}
