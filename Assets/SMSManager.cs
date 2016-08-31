using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SMSManager : MonoBehaviour {
	public int pledgeCount;

	[SerializeField] bool countedPledges = false;
	[SerializeField] bool loadedPledges = false;
	private bool calledLoad = false;

	private const string ParseKey = "Pledge2254:";
	private const float PingWaitTime = 5f;

	private const string COUNT_URL = "http://wence.herokuapp.com/get_pledge_count";
	private const string LOAD_URL = "http://wence.herokuapp.com/last_100";
	private const string PERIODIC_URL = "http://wence.herokuapp.com/last_minute";

	string[] stock_pledges = new string[] {
		"Pledge2254:0:I pledge to take 5 minute showers.", 
		"Pledge2254:0:I will only buy seafood from sustainable fisheries.", 
		"Pledge2254:0:Nex time I go to the beach, I am going to meditate and take in the beauty.", 
		"Pledge2254:0:I will go surfing more and become closer to the sea."
	};


	public static SMSManager Instance;

	private HashSet<int> usedRealPledgeIDs = new HashSet<int> ();
	private List<SMSData> pledges = new List<SMSData>();
	private List<SMSData> recentPledges = new List<SMSData>();

	public class SMSData {
		public int id;
		public string pledge;

		public SMSData(int i, string p) {
			id = i;
			pledge = p;
		}

		public String ToString() {
			return "ID: " + id + "\n" + "Pledge: " + pledge + "\n";
		}
	}

	void Awake() {
		if (Instance == null)
			Instance = this;

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
				print (pledges [i].pledge);
			}
			print ("Recent pledges:");
			for (int i = 0; i < recentPledges.Count; i++) {
				print (recentPledges [i].pledge);
			}
		}

		if (Input.GetKeyDown (KeyCode.G)) {
			print (GetRandomPledgeText ());
		}
	}

	void OnDestroy() {
		if (Instance == this)
			Instance = null;
	}

	IEnumerator GetPledgeCount() {
		WWW www = new WWW (COUNT_URL);
		yield return www;
		yield return new WaitForFixedUpdate (); //align with update frame
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
			string[] messageSplit = text.Split (new char[] { ':' }, 2);
			SMSData sData = new SMSData (int.Parse(messageSplit[0]), messageSplit [1]);

			if (parsingRecentPledges) {
				if (usedRealPledgeIDs.Contains (sData.id))
					continue;
				recentPledges.Add (sData);
			} else {
				pledges.Add (sData);
			}
			usedRealPledgeIDs.Add (sData.id);				
		}
	}

	public string GetRandomPledgeText () {
		List<SMSData> pledgeSource = pledges;
		if (recentPledges.Count != 0) {
			pledgeSource = recentPledges;
		}
		
		int index = UnityEngine.Random.Range (0, pledgeSource.Count-1);
		SMSData sData = pledgeSource [index];
		pledgeSource.RemoveAt (index);
		return sData.pledge;
	}
}
