using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

    public Transform spawnPoint;
    public GameObject[] spawnable;
    public float spawnTime;

    float time = 0;


    // Use this for initialization
    void Start () {
       

	}
	
	// Update is called once per frame
	void FixedUpdate() {

        time += Time.deltaTime;
        if(time > spawnTime)
        {
            int num = (int) Random.Range(0.0f, spawnable.Length);
            time = 0;
            Instantiate(spawnable[num], spawnPoint.position, spawnPoint.rotation);

        }
        

    }
}
