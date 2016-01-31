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
            stayCounter += Time.deltaTime;
        }

        if(stayCounter >= 1)
        {
            ServiceLocator.GetGameManager().WinLevelEarly();
        }


	}

    void OnTriggerEnter(Collider collider)
    {
        isColliding = true;
    }

    void OnTriggerExit(Collider collider)
    {
        isColliding = false;
        stayCounter = 0;
    }
}
