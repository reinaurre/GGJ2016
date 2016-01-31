using UnityEngine;
using System.Collections;

public class PlatformSpawnerScript : MonoBehaviour
{
	public GameObject platform;
	public GameObject robot;
	public int randomNumber;
	public float currentSpawnPosition = 30;
	public GameObject[] Platforms;

	// Use this for initialization
	public float PlatformSizeZ ;
	void Start () 
	{
		robot = GameObject.FindGameObjectWithTag("Player");
		PlayerScript PlayerSCR= robot.GetComponent<PlayerScript>();
		PlatformSizeZ = platform.transform.localScale.z;
		InvokeRepeating("SpawnPlatforms", 0, 5.0f/PlayerSCR.speedFwd);
	}
	
	// Update is called once per frame
	void Update () 
	{
		//PlayerScript PlayerSCR= robot.GetComponent<PlayerScript>();
	}


	void SpawnPlatforms()
	{
        randomNumber = Random.Range(0,4);
		Instantiate (Platforms[randomNumber], platform.transform.position + new Vector3 (0, 0, currentSpawnPosition), platform.transform.rotation);
		currentSpawnPosition+=20;		
	}
}
