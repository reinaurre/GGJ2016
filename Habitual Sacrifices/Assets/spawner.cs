using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

    public Transform spawnPoint;
    public GameObject[] spawnable;
    public float spawnTime;
    public float maxSpeedUp = 3.0f;

    float time = 0;
	
	void FixedUpdate() {
        float speedFactor = ServiceLocator.GetGameManager().GetSpeedFactor(maxSpeedUp);

        time += speedFactor;
        if(time > spawnTime)
        {
            int num = (int) Random.Range(0.0f, spawnable.Length);
            time = 0;
            Instantiate(spawnable[num], spawnPoint.position, spawnPoint.rotation);

        }
        

    }
}
