using UnityEngine;
using System.Collections;

public class chickenCollisions : MonoBehaviour {

    public string fail;
    public string pass;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject.tag.Equals(fail))
        {
            ServiceLocator.GetGameManager().LoseLife();
        }
        if(other.gameObject.tag.Equals(pass))
        {
            ServiceLocator.GetGameManager().IncrementScore(20);
        }
    }
}
