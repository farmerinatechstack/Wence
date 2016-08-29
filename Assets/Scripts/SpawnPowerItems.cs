using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/* Class: SpawnPowerItems
 * Spawns the power-based items in the environment */
public class SpawnPowerItems : MonoBehaviour {
	[SerializeField] float spawnRadius;
	[SerializeField] private int numSpawnItems;

	[SerializeField] List<GameObject> trashPowerItems;
	[SerializeField] List<GameObject> lowPowerItems;
	[SerializeField] List<GameObject> highPowerItems;
	private int powerThreshold = 700;
	private List<GameObject> powerItems;

	private void Awake() {
		powerItems = new List<GameObject> (lowPowerItems);
		if (ExperienceManager.Power >= powerThreshold) powerItems.AddRange (highPowerItems);
	}

	private void OnEnable() {
		ExperienceManager.StartListening (ExperienceManager.INFO_LOADED, SpawnItems);
	}

	private void OnDisable() {
		ExperienceManager.StopListening (ExperienceManager.INFO_LOADED, SpawnItems);
	}

	/* Function: SpawnItems
	 * Spawns at most numSpawnItems in spawnRadius around centerPosition.
	 * Fewer items may be spawned if no space can be found in the spawn circle. */
	private void SpawnItems() {
		for (int i = 0; i < numSpawnItems; i++) {
			GameObject powerItem = GetPowerItem ();
			PowerItem objProperties = powerItem.GetComponent<PowerItem> ();

			Vector3 spawnPosition = GetSpawnPosition (objProperties);
			if (spawnPosition != Vector3.zero) {
				GameObject spawnedObj = Instantiate (powerItem, spawnPosition, objProperties.getRotation()) as GameObject;
				spawnedObj.transform.parent = transform;
			}
		}
	}

	private GameObject GetPowerItem() {
		if (Random.Range(0, ExperienceManager.MAX_POWER) <= ExperienceManager.Power) {
			return powerItems[Random.Range(0,powerItems.Count)];
		} else {
			return trashPowerItems[Random.Range(0,trashPowerItems.Count)];
		}
	}

	/* Function: GetSpawnPosition
	 * If possible, returns a spawn position for the provided GameObject that does not 
	 * overlap with colliders already in the scene. */
	private Vector3 GetSpawnPosition(PowerItem objProperties) {
		float yOffset = objProperties.getYOffset();
		float objectRadius = objProperties.getObjRadius();

		float startTime = Time.realtimeSinceStartup;
		Vector3 spawnPosition = new Vector3();
		bool positionFound = false;
		while (!positionFound) { // look for a spawn position
			Vector2 rawPosition = Random.insideUnitCircle * spawnRadius;
			spawnPosition = new Vector3 (rawPosition.x, yOffset, rawPosition.y) + transform.position;
			positionFound = !Physics.CheckSphere (spawnPosition, objectRadius);
			if (Time.realtimeSinceStartup - startTime > 0.5f) return Vector3.zero;
		}
		return spawnPosition;
	}
}
