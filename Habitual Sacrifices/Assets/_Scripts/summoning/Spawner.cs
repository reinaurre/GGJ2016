using UnityEngine;

[System.Serializable]
class SpawnGroup {
    public Transform[] prefabs = null;
}

class Spawner : MonoBehaviour {
    public SpawnGroup[] spawnGroups = null;

    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1.0f;
    public float spawnOffset = 10.0f;
    public float maxSpeedUp = 3.0f;

    private float spawnTimer = 0.0f;
    private float spawnTime = 0.0f;

    void Awake() {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update() {
        GameManager manager = ServiceLocator.GetGameManager();
        float speedFactor = manager.GetSpeedFactor(maxSpeedUp);

        spawnTimer += speedFactor;
        if (spawnTimer >= spawnTime) {
            Vector3 spawnPosition = transform.position;
            spawnPosition.x += Random.Range(-spawnOffset, spawnOffset);

            SpawnGroup groupToSpawn = Util.randomElemIn(spawnGroups);
            Transform thingToSpawn = Util.randomElemIn(groupToSpawn.prefabs);
            Instantiate(thingToSpawn, spawnPosition, Quaternion.identity);

            spawnTimer -= spawnTime;
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
}
