using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
class SpawnGroup {
    public bool good = true;
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

    public class ObjectInCauldronEvent : UnityEvent<Vector3,bool> {};
    public ObjectInCauldronEvent OnObjectInCauldron = new ObjectInCauldronEvent();

    void Awake() {
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        OnObjectInCauldron.AddListener(AddScoreIfGood);
        ServiceLocator.GetSoundSystem().PlaySound("hintSummoning");
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
            Transform prefab = Instantiate(thingToSpawn, spawnPosition, Quaternion.identity) as Transform;

            prefab.gameObject.AddComponent<FallingThing>();
            prefab.gameObject.AddComponent<HitReceiver>();
            if (groupToSpawn.good) {
                GoodHitHandler handler = prefab.gameObject.AddComponent<GoodHitHandler>();
                handler.OnInCauldron.AddListener(x => OnObjectInCauldron.Invoke(x, true));
            } else {
                BadHitHandler handler = prefab.gameObject.AddComponent<BadHitHandler>();
                handler.OnInCauldron.AddListener(x => OnObjectInCauldron.Invoke(x, false));
            }

            spawnTimer -= spawnTime;
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }

    void AddScoreIfGood(Vector3 position, bool good) {
        GameManager manager = ServiceLocator.GetGameManager();
        if (good) {
            manager.IncrementScore(100.0f);
        }
    }
}
