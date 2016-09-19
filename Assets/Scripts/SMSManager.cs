using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SMSManager : MonoBehaviour {
	public static int pledgeCount;

	[SerializeField] bool countedPledges = false;
	[SerializeField] bool loadedPledges = false;
	private bool calledLoad = false;

	private const string ParseKey = "Pledge2254:";
	private const float PingWaitTime = 5f;

	private const string COUNT_URL = "http://wence.herokuapp.com/get_pledge_count";
	private const string LOAD_URL = "http://wence.herokuapp.com/last_100";
	private const string PERIODIC_URL = "http://wence.herokuapp.com/last_minute";

	string[] stock_pledges = new string[] {
		"Pledge2254:0:I pledge to take 5 minute showers.:08/30/2016", 
		"Pledge2254:0:I will only buy seafood from sustainable fisheries.:08/30/2016", 
		"Pledge2254:0:Next time I go to the beach, I am going to meditate and take in the beauty.:08/30/2016", 
		"Pledge2254:0:I will go surfing more and become closer to the sea.:08/30/2016",
		"Pledge2254:0:I am going to fish so I can understand more of the issues.:08/30/2016",
		"Pledge2254:0:Next time I drink water, I'll think about where it comes from.:08/30/2016"

	};


	public static SMSManager instance {
		get {
			if (!smsManager) {
				smsManager = FindObjectOfType (typeof(SMSManager)) as SMSManager;
			}
			return smsManager;
		}
	}
	private static SMSManager smsManager;

	private static HashSet<int> usedRealPledgeIDs = new HashSet<int> ();
	private static List<SMSData> pledges = new List<SMSData>();
	private static List<SMSData> recentPledges = new List<SMSData>();

	public class SMSData {
		public int id;
		public string pledge;
		public string time;

		public SMSData(int i, string p, string d) {
			id = i;
			pledge = p;

			DateTime dateTime = Convert.ToDateTime(d);
			if (dateTime == DateTime.Today) {
				time = "Today";
			} else {
				time = dateTime.Month + "/" + dateTime.Day;
			}
		}

		public String ToString() {
			return "ID: " + id + "\n" + "Pledge: " + pledge + "\n";
		}
	}

	void Awake() {
		DontDestroyOnLoad (transform.gameObject);

		for (int i = 0; i < stock_pledges.Length; i++) {
			ParseData (stock_pledges [i], false);
		}
	}

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.WAITING_FOR_PLEDGES, ActivateSMSLoads);
		ExperienceManager.StartListening (ExperienceManager.WAITING_FOR_SHIFT, AlertOnLoad);

	}

	private void OnDisable() {
		ExperienceManager.StopListening (ExperienceManager.WAITING_FOR_PLEDGES, ActivateSMSLoads);
		ExperienceManager.StartListening (ExperienceManager.WAITING_FOR_SHIFT, AlertOnLoad);

	}

	void Update() {
		if (Input.GetKeyDown (KeyCode.P)) {
			print ("Loaded pledges:");
			for (int i = 0; i < pledges.Count; i++) {
				Debug.Log (pledges [i].pledge + " - " + pledges [i].time);
			}
			print ("Recent pledges:");
			for (int j = 0; j < recentPledges.Count; j++) {
				Debug.Log (recentPledges [j].pledge + " - " + pledges [j].time);
			}
		}
	}

	private void AlertOnLoad() {
		StartCoroutine (WaitForLoad ());
	}

	IEnumerator WaitForLoad() {
		while (!countedPledges || !loadedPledges) {
			yield return new WaitForSeconds (1f);
		}

		ExperienceManager.TriggerEvent (ExperienceManager.INFO_LOADED);
	}

	private void ActivateSMSLoads() {
		Debug.Log ("Activating SMS Coroutines");

		StartCoroutine (LoadCount ());
		StartCoroutine (LoadPledges ());
		StartCoroutine (LoadPeriodically ());
	}

	IEnumerator LoadCount() {				// Loads total count of pledges.
		WWW www = new WWW (COUNT_URL);
		yield return www;

		pledgeCount = int.Parse (www.text);
		countedPledges = true;
	}

	IEnumerator LoadPledges() { 			// Loads the last 100 pledges.
		WWW www = new WWW (LOAD_URL);
		yield return www;
		ParseData (www.text, false);
		loadedPledges = true;
	}

	IEnumerator LoadPeriodically() { 		// Loads new pledges periodically.
		while (true) {
			WWW www = new WWW (PERIODIC_URL);
			yield return www;
			ParseData (www.text, true);
			yield return new WaitForSeconds (PingWaitTime);
		}
	}

	/* Function: ParseData
	 * Parses a string of pledge data. If parsingRecentPledges is true, the pledges are added to the list of recents. */
	private void ParseData(string data, bool parsingRecentPledges) {
		string[] texts = data.Split(new string[] { ParseKey }, StringSplitOptions.None);
		for(int i = 1; i < texts.Length; i++) { //skip the first one empty space
			string text = texts [i];
			string[] messageSplit = text.Split (new char[] { ':' }, 3);
			SMSData sData = new SMSData (int.Parse(messageSplit[0]), messageSplit [1], messageSplit[2]);

			if (parsingRecentPledges) {
				if (usedRealPledgeIDs.Contains (sData.id))
					continue;
				sData.time = "Just Now";
				recentPledges.Add (sData);
				ExperienceManager.TriggerEvent (ExperienceManager.HANDLE_RECENT_PLEDGE);
			} else {
				pledges.Add (sData);
			}
			usedRealPledgeIDs.Add (sData.id);				
		}
	}

	public static SMSData GetRandomPledgeText () {
		if (recentPledges.Count == 0 && pledges.Count == 0) {
			return null;
		}

		int index;
		SMSData sData;
		if (recentPledges.Count != 0) {
			index = UnityEngine.Random.Range (0, recentPledges.Count);
			sData = recentPledges [index];
			recentPledges.RemoveAt (index);
		} else {
			index = UnityEngine.Random.Range (0, pledges.Count);
			sData = pledges [index];
			pledges.RemoveAt (index);
		}		
		return sData;
	}
}
