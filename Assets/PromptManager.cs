using UnityEngine;
using System.Collections;

public class PromptManager : MonoBehaviour {
	void Awake () {
		RenderSettings.fog = false;
	}

	void OnDestroy() {
		RenderSettings.fog = true;
	}

	void Start() {
		ExperienceManager.TriggerEvent (ExperienceManager.WAITING_FOR_PLEDGES);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
