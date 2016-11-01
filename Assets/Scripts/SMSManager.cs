using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;

public class SMSManager : MonoBehaviour {
	public static SMSManager instance;

	public int Count;
	public int Power;
	public int MAX_POWER = 1000;
	public float PowerRatio;

	private bool gotCount = false;
	private bool gotPower = false;
	private bool loadedPledges = false;

	private HashSet<int> usedRealPledgeIDs;
	private List<SMSData> pledges;
	private List<SMSData> recentPledges;

	// SMS Constants
	private static string ParseKey = "Pledge2254:";
	private static float PingWaitTime = 10f;
	private static string COUNT_URL = 	"http://wence.herokuapp.com/get_pledge_count";
	private static string POWER_URL = 	"http://wence.herokuapp.com/get_power_exponential";
	private static string LOAD_URL = 	"http://wence.herokuapp.com/last_100";
	private static IEnumerator countC;
	private static IEnumerator powerC;
	private static IEnumerator loadC;
	private static string[] stock_pledges = new string[] {
		"Pledge2254:0:I promise to take 5 minute showers so I can help out the oceans.:10/15/2016", 
		"Pledge2254:0:I love the sound of the waves on the rocks near my home.:10/22/2016", 
		"Pledge2254:0:Next time I go to the beach, I am going to meditate and take in the beauty.:10/5/2016", 
		"Pledge2254:0:I will go surfing more and become closer to the sea.:11/1/2016",
		"Pledge2254:0:I got married on a beach. Oceans are the most beautiful part of the world.:10/17/2016",
		"Pledge2254:0:There's something special about water. It's important for all life and we need to protect our waters.:08/30/2016"
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
		if (instance == null) {
			instance = this;
			usedRealPledgeIDs = new HashSet<int> ();
			pledges = new List<SMSData> ();
			recentPledges = new List<SMSData> ();
		} else {
			Debug.Log("Destroying GameObject with duplicate SMSManager: " + gameObject.name);
			Destroy (gameObject);
		}
	}

	void Start() {
		for (int i = 0; i < stock_pledges.Length; i++) {
			ParseData (stock_pledges [i], false);
		}

		ActivateSMSLoads ();
	}

	private void ActivateSMSLoads() {
		gotCount = false;
		gotPower = false;
		loadedPledges = false;

		Debug.Log ("Activating SMS Coroutines");
		StartCoroutine (SetPower ());
		countC = LoadCount ();
		loadC = LoadPledges ();
		powerC = LoadPower ();
		StartCoroutine (countC);
		StartCoroutine (loadC);
		StartCoroutine (powerC);
	}


	IEnumerator SetPower() {
		#if !UNITY_EDITOR
		yield return new WaitForSeconds (30f); // Wait 30s for WWW connection results
		#endif

		yield return null;
		StopCoroutine(countC);
		StopCoroutine(loadC);
		StopCoroutine(powerC);

		Debug.Log ("Populating SMS Data");
		if (gotCount && gotPower && loadedPledges) {
			Power = Power > 600 ? Power : (600 + Power);
			Power = Mathf.Clamp (Power, 0, MAX_POWER);
			PowerRatio = (float)Power / (float)MAX_POWER;
		} else {
			Count = UnityEngine.Random.Range(20, 40);
			Power = UnityEngine.Random.Range(600, 800);
			PowerRatio = UnityEngine.Random.Range (0.6f, 0.8f);
		}
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

	public SMSData GetRandomPledgeText () {
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
