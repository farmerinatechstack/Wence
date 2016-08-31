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
	public static float PowerRatio;


	// Events: the following are string names for all possible events in the experience
	public static string INFO_LOADED = "infoLoaded";
	public static string ENVIRONMENT_SET = "environmentSet";
	public static string PLEDGE_SELECTED = "pledgeSelected";
	public static string PLEDGE_HIDDEN = "pledgeHidden";

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

	private void OnEnable() {
		StartListening (ExperienceManager.INFO_LOADED, SetPower);
	}

	private void OnDisable() {
		StopListening (ExperienceManager.INFO_LOADED, SetPower);
	}

	/* Function: Update
	 * Used to mock events for testing */
	private void Update() {
		
	}

	private void Init() {
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent> ();
		}
	}

	/* Function: SetPower
	 * Sets the power and power percentage of the environment */
	private void SetPower() {
		Power = SMSManager.pledgeCount + 200;
		PowerRatio = (float)Power / (float)MAX_POWER;
	}

	public static int GetPower() {
		return Power;
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
