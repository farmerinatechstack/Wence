using UnityEngine;
using System.Collections;

public class GodRayCycle : MonoBehaviour {
	public Texture[] textures;

	private Renderer r;

	// Use this for initialization
	void Start () {
		r = gameObject.GetComponent<Renderer> ();
		StartCoroutine(DoTextureLoop());
	}

	public IEnumerator DoTextureLoop(){
		int i = 0;
		int addFactor = 1;
		while (true) {
			i += addFactor;
			if (i == 0 || i == textures.Length - 1) addFactor *= -1;

			r.material.mainTexture = textures[i];

			if (i == textures.Length - 1) {
				yield return new WaitForSeconds (Random.Range (1.0f, 4.0f));
			} else {
				yield return new WaitForSeconds (Random.Range (0.03f, 0.1f));
			}
		}
	}
}
