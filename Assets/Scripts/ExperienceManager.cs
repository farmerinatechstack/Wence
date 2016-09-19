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

	[SerializeField] int powerOffset;

	// Events: the following are string names for all possible events in the experience
	public static string INFO_LOADED = "infoLoaded";
	public static string ENVIRONMENT_SET = "environmentSet";
	public static string HANDLE_RECENT_PLEDGE = "handleRecentPledge";
	public static string PLEDGE_SELECTED = "pledgeSelected";
	public static string PLEDGE_HIDDEN = "pledgeHidden";
	public static string WAITING_FOR_PLEDGES = "waitingForPledges";
	public static string WAITING_FOR_SHIFT = "waitingForShift";


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

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);
	}

	private void OnEnable() {
		StartListening (ExperienceManager.INFO_LOADED, SetPower);
		StartListening (ExperienceManager.HANDLE_RECENT_PLEDGE, HandleRecent);
	}

	private void OnDisable() {
		StopListening (ExperienceManager.INFO_LOADED, SetPower);
		StartListening (ExperienceManager.HANDLE_RECENT_PLEDGE, HandleRecent);
	}

	private void Init() {
		if (eventDictionary == null) {
			eventDictionary = new Dictionary<string, UnityEvent> ();
		}
	}

	/* Function: SetPower
	 * Sets the power and power ratio of the environment */
	private void SetPower() {
		Power = SMSManager.pledgeCount + powerOffset;
		PowerRatio = (float)Power / (float)MAX_POWER;
	}

	/* Function: SetPower
	 * Handles the detection of a recent pledge */
	private void HandleRecent() {
		Power += 500;
		PowerRatio += 0.5f;
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
