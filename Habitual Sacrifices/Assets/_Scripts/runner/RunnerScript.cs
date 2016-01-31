using UnityEngine;
using System.Collections;

public class RunnerScript : MonoBehaviour {
	public Vector3 maxVelocity;// = new Vector3 (5, 7,10);
	public float cameraSpeed ;
	public GameObject robot;
	public Transform cameraPos;

	void FixedUpdate ()
	{
		transform.position = new Vector3(transform.position.x,transform.position.y,robot.transform.position.z -10 );
	}
}
