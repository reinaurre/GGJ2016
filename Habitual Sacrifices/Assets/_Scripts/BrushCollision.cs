using UnityEngine;
using System.Collections;

public class BrushCollision : MonoBehaviour
{
    private float stayCounter;
    private bool isColliding;
    private bool shouldGrunt = false;

	// Use this for initialization
	void Start ()
    {
        stayCounter = 0;
        isColliding = false;
        ServiceLocator.GetGameManager().winOnTimeOut = false;
        ServiceLocator.GetSoundSystem().PlayBackgroundMusic("morning");
        ServiceLocator.GetSoundSystem().PlaySound("hintBrush");
	}
	
	// Update is called once per frame
	void Update ()
    {

      if (!shouldGrunt) {
        ServiceLocator.GetSoundSystem().PlaySound("grunt");
        shouldGrunt = true;
      }

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
