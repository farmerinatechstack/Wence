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

	private void Start() {
		Power = GetPower ();
		PowerRatio = (float)Power / (float)MAX_POWER;
		TriggerEvent (INFO_LOADED);
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

	/* Function: GetPower
	 * Returns the power of the environment */
	private int GetPower() {
		Power = SMSManager.Instance.pledgeCount;
		return Power + 200;
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
		print ("Triggering: " + eventName);

		UnityEvent thisEvent = null;
		if (instance.eventDictionary.TryGetValue (eventName, out thisEvent)) {
			thisEvent.Invoke ();
		}
	}
}
