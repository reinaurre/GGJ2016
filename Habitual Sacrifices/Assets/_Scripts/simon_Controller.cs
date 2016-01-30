using UnityEngine;
using System.Collections;

public class simon_Controller : MonoBehaviour {

	public GameObject objectCam;
	public GameObject objectCell;

	Animator objectAnim;
	Animator objectCamAnim;

	// Use this for initialization
	void Start () {
	
		objectAnim = objectCell.GetComponent <Animator> ();
		objectCamAnim = objectCam.GetComponent <Animator> ();

	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetButtonDown ("Fire1")) {
			objectAnim.SetTrigger ("blur_rotation");
			objectCamAnim.SetTrigger ("blur_rotation");
		}

	}
}
