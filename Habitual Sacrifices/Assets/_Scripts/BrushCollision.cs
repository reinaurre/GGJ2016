using UnityEngine;
using System.Collections;

public class BrushCollision : MonoBehaviour
{
    private float stayCounter;
    private bool isColliding;

	// Use this for initialization
	void Start ()
    {
        stayCounter = 0;
        isColliding = false;
        ServiceLocator.GetGameManager().winOnTimeOut = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(isColliding)
        {
            Debug.Log("colliding");
            stayCounter += Time.deltaTime;
        }

        if(stayCounter >= 1)
        {
            Debug.Log("win brush");
            ServiceLocator.GetGameManager().WinLevelEarly();
        }


	}

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("In Trigger");
        isColliding = true;
    }

    void OnTriggerExit(Collider collider)
    {
        Debug.Log("Left Trigger");
        isColliding = false;
        stayCounter = 0;
    }
}
