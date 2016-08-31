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

			print(d + ": " + p);
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
		for (int i = 0; i < stock_pledges.Length; i++) {
			ParseData (stock_pledges [i], false);
		}

		StartCoroutine (GetPledgeCount ());
		StartCoroutine (LoadPledges ());
		StartCoroutine (CheckPledgesPeriodically ());
	}

	void Update() {
		if (countedPledges && loadedPledges && !calledLoad) { 	// Alert rest of the scene that information has been loaded
			ExperienceManager.TriggerEvent (ExperienceManager.INFO_LOADED);
			calledLoad = true;
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			print ("Loaded pledges:");
			for (int i = 0; i < pledges.Count; i++) {
				print (pledges [i].pledge + " - " + pledges [i].time);
			}
			print ("Recent pledges:");
			for (int j = 0; j < recentPledges.Count; j++) {
				print (recentPledges [j].pledge + " - " + pledges [j].time);
			}
		}
	}

	IEnumerator GetPledgeCount() {
		WWW www = new WWW (COUNT_URL);
		yield return www;
		yield return new WaitForFixedUpdate (); //align with update frame

		pledgeCount = int.Parse (www.text);
		countedPledges = true;
	}

	IEnumerator LoadPledges() {
		WWW www = new WWW (LOAD_URL);
		yield return www;
		yield return new WaitForFixedUpdate (); //align with update frame
		ParseData (www.text, false);
		loadedPledges = true;
	}

	IEnumerator CheckPledgesPeriodically() {
		while (true) {
			WWW www = new WWW (PERIODIC_URL);
			yield return www;
			yield return new WaitForFixedUpdate (); //align with update frame
			ParseData (www.text, true);
			yield return new WaitForSeconds (PingWaitTime);
		}
	}

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
			} else {
				pledges.Add (sData);
			}
			usedRealPledgeIDs.Add (sData.id);				
		}
	}

	public static SMSData GetRandomPledgeText () {
		print ("Getting text");

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
