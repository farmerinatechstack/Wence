using UnityEngine;
using System.Collections;

/* Class: SpawnPowerItems
 * Spawns the power-based items in the environment */
public class SpawnPowerItems : MonoBehaviour {
	[SerializeField] float spawnRadius;
	[SerializeField] private int numSpawnItems;
	
	private void OnEnable() {
		EventManager.StartListening (EventManager.INFO_LOADED, SpawnItems);
	}

	private void OnDisable() {
		EventManager.StopListening (EventManager.INFO_LOADED, SpawnItems);
	}

	/* Function: SpawnItems
	 * Spawns at most numSpawnItems in spawnRadius around centerPosition.
	 * Fewer items may be spawned if no space can be found in the spawn circle. */
	private void SpawnItems() {
		// TODO: read pledge power to filter items

		for (int i = 0; i < numSpawnItems; i++) {
			// TODO: randomly choose item to spawn
			GameObject powerItem = GameObject.CreatePrimitive(PrimitiveType.Cube);

			Vector3 spawnPosition = GetSpawnPosition (powerItem);
			if (spawnPosition != Vector3.zero) {
				Instantiate (powerItem, spawnPosition, Random.rotation);
			}
		}
	}

	/* Function: GetSpawnPosition
	 * If possible, returns a spawn position for the provided GameObject that does not 
	 * overlap with colliders already in the scene. The y-position is determined by the
	 * GameObject because each has a different required offset. */
	private Vector3 GetSpawnPosition(GameObject powerItem) {
		// TODO: pass in GameObject, customize by GameObject y-offset & objectRadius
		float yOffset = 0f;
		float objectRadius = 0.5f;

		float startTime = Time.realtimeSinceStartup;
		Vector3 spawnPosition = new Vector3();
		bool positionFound = false;
		while (!positionFound) {
			Vector2 rawPosition = Random.insideUnitCircle * spawnRadius;
			spawnPosition = new Vector3 (rawPosition.x, yOffset, rawPosition.y) + transform.position;
			positionFound = !Physics.CheckSphere (spawnPosition, objectRadius);
			if (Time.realtimeSinceStartup - startTime > 0.5f) return Vector3.zero;
		}
		print ("found spot");
		return spawnPosition;
	}
}
