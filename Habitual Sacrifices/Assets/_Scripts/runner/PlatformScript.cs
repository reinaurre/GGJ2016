using UnityEngine;
using System.Collections;

public class PlatformScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine("Destroy");
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

	IEnumerator Destroy()
	{
		yield return new WaitForSeconds (20f); 
		{
						//Destroy (this); this destroys the script
						Destroy (gameObject);
		}
	}
}
