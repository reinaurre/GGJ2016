using UnityEngine;
using System.Collections;

public class LampPush : MonoBehaviour
{

    private Rigidbody rb;

	void Awake ()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(new Vector3(500, 0, -500));
	}

    private float count = 0;
    private bool stop = false;
	
	// Update is called once per frame
	void Update ()
    {
	    if (count < 1)
        {
            count++;
        }

        if(!stop)
        {
            rb.AddForce(new Vector3(-200, 0, 0));
            stop = true;
        }
	}
}
