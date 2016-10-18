using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SMSManager : MonoBehaviour {
	public static int Count;
	public static int Power;
	public const int MAX_POWER = 1000;
	public static float PowerRatio;

	private static bool gotCount = false;
	private static bool gotPower = false;
	private static bool loadedPledges = false;

	private static SMSManager instance;
	private static HashSet<int> usedRealPledgeIDs = new HashSet<int> ();
	private static List<SMSData> pledges = new List<SMSData>();
	private static List<SMSData> recentPledges = new List<SMSData>();

	// SMS Constants
	private const string ParseKey = "Pledge2254:";
	private const float PingWaitTime = 10f;
	private const string COUNT_URL = 	"http://wence.herokuapp.com/get_pledge_count";
	private const string POWER_URL = 	"http://wence.herokuapp.com/get_power_exponential";
	private const string LOAD_URL = 	"http://wence.herokuapp.com/last_100";
	private static IEnumerator countC;
	private static IEnumerator powerC;
	private static IEnumerator loadC;
	private const string PERIODIC_URL = "http://wence.herokuapp.com/last_minute";
	string[] stock_pledges = new string[] {
		"Pledge2254:0:I pledge to take 5 minute showers.:08/30/2016", 
		"Pledge2254:0:I will only buy seafood from sustainable fisheries.:08/30/2016", 
		"Pledge2254:0:Next time I go to the beach, I am going to meditate and take in the beauty.:08/30/2016", 
		"Pledge2254:0:I will go surfing more and become closer to the sea.:08/30/2016",
		"Pledge2254:0:I am going to fish for my own food.:08/30/2016",
		"Pledge2254:0:Next time I drink water, I won't waste any.:08/30/2016"
	};

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
				time = dateTime.Month + "/" + dateTime.Day + "/16";
			}
		}
	}

	void Awake() {
		if (!instance) {
			instance = this;
		} else {
			Debug.LogError ("Destroying gameObject with duplicate SMSManager instance: " + gameObject.name);
			Destroy (gameObject);
		}
	}

	void Start() {
		for (int i = 0; i < stock_pledges.Length; i++) {
			ParseData (stock_pledges [i], false);
		}
		ActivateSMSLoads ();
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

	private void ActivateSMSLoads() {
		Debug.Log ("Activating SMS Coroutines");

		StartCoroutine (SetPower ());

		countC = LoadCount ();
		loadC = LoadPledges ();
		powerC = LoadPower ();
		StartCoroutine (countC);
		StartCoroutine (loadC);
		StartCoroutine (powerC);

		StartCoroutine (LoadPeriodically ());
	}


	IEnumerator SetPower() {
		yield return new WaitForSeconds (30f); // Wait for WWW connection to be established and read
		StopCoroutine(countC);
		StopCoroutine(loadC);
		StopCoroutine(powerC);


		if (gotCount && gotPower && loadedPledges) {
			Power = Power > 500 ? Power : (300 + Power);
			Power = Mathf.Clamp (Power, 0, MAX_POWER);
			PowerRatio = (float)Power / (float)MAX_POWER;
		} else {
			Count = UnityEngine.Random.Range(15, 25);
			Power = UnityEngine.Random.Range(400, 600);
			PowerRatio = UnityEngine.Random.Range (0.4f, 0.6f);
		}

		Debug.Log("Count: " + Count.ToString());
		Debug.Log("Power: " + Power.ToString());
	}

	IEnumerator LoadCount() {				// Loads total count of pledges.
		WWW www = new WWW (COUNT_URL);
		yield return www;

		Count = int.Parse (www.text);
		gotCount = true;
	}

	IEnumerator LoadPower() {				// Loads total count of pledges.
		WWW www = new WWW (POWER_URL);
		yield return www;

		Power = int.Parse (www.text);
		gotPower = true;
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
