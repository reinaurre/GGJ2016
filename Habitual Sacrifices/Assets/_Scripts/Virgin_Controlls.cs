using UnityEngine;
using System.Collections;

public class Virgin_Controlls : MonoBehaviour {

    public Transform flicker;
    float axisValue;
	// Use this for initialization
	void Start () {
        ServiceLocator.GetSoundSystem().PlaySound("hintVirgins");

        Vector3 rotation = flicker.rotation.eulerAngles;
        rotation.x = 45;
        flicker.rotation = Quaternion.Euler(rotation);
	}
	
	// Update is called once per frame
	void Update () {

        axisValue = Input.GetAxis("Horizontal");
        if(axisValue != 0)
        {
            Vector3 rotation = flicker.rotation.eulerAngles;
            rotation.x = 45 * Mathf.Sign(axisValue);
            flicker.rotation = Quaternion.Euler(rotation);
        }
    }
}
