using UnityEngine;
using System.Collections;

public class Virgin_Controlls : MonoBehaviour {
    public Transform flicker;
    float prevAxisValue;
    float axisValue;

	// Use this for initialization
	void Start () {
        this.enabled = false;
        ServiceLocator.GetGameManager().OnLevelBegin.AddListener(() => (this.enabled = true));
        ServiceLocator.GetGameManager().OnLevelEnd.AddListener((x) => (this.enabled = false));

        ServiceLocator.GetSoundSystem().PlaySound("hintVirgins");

        Vector3 rotation = flicker.rotation.eulerAngles;
        rotation.x = 45;
        flicker.rotation = Quaternion.Euler(rotation);
	}
	
	// Update is called once per frame
	void Update () {
        axisValue = Input.GetAxis("Horizontal");
        if (Mathf.Abs(prevAxisValue) < 0.3f && Mathf.Abs(axisValue) >= 0.3f) {
            Vector3 rotation = flicker.rotation.eulerAngles;
            rotation.x = 45 * Mathf.Sign(axisValue);
            flicker.rotation = Quaternion.Euler(rotation);

            ServiceLocator.GetSoundSystem().PlaySound("paddle");
        }
        prevAxisValue = axisValue;
    }
}
