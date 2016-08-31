using UnityEngine;
using System;

namespace VRAssets {
	// Encapsulates VR inputs.
	public class VRInput : MonoBehaviour {
		public event Action Back;		// Called on press down of back button
		public event Action OnDown; 	// Called on press down of Fire1
		public event Action OnUp; 		// Called on release of Fire1

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			CheckInput();
		}

		private void CheckInput() {
			if (Input.GetButtonDown ("Fire1")) { 	// Touchpad down
				if (OnDown != null)
					OnDown ();
			}

			if (Input.GetButtonUp ("Fire1")) {		// Touchpad up
				if (OnUp != null)
					OnUp ();
			} 

			if (Input.GetKeyDown(KeyCode.Escape)) {	// Back button down
				if (Back != null) 
					Back ();
			}
		}

		private void OnDestroy() {
			OnDown = null;
			OnUp = null;
		}
	}
}
