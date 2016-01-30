using UnityEngine;

class Spawner : MonoBehaviour {
    public Transform[] thingsToSpawn = null;

    public float minSpawnTime = 0.5f;
    public float maxSpawnTime = 1.0f;
    public float spawnOffset = 10.0f;

    private float spawnTimer = 0.0f;
    private float spawnTime = 0.0f;

    void Awake() {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    void Update() {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnTime) {
            Vector3 spawnPosition = transform.position;
            spawnPosition.x += Random.Range(-spawnOffset, spawnOffset);

            Transform thingToSpawn = Util.randomElemIn(thingsToSpawn);
            Instantiate(thingToSpawn, spawnPosition, Quaternion.identity);

            spawnTimer -= spawnTime;
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
}
