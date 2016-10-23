using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class DontDestroy : MonoBehaviour {
	
	void Start () {
		DontDestroyOnLoad (gameObject);
	}
}
