using UnityEngine;
using System.Collections;

public class Virgin_Controlls : MonoBehaviour {
    public Rigidbody flicker;
    public float rotationAngle = 35.0f;
    public float rotationTime = 0.1f;

    float prevAxisValue;
    float axisValue;

    Quaternion targetRotation = Quaternion.identity;

	// Use this for initialization
	void Start () {
        this.enabled = false;
        ServiceLocator.GetGameManager().OnLevelBegin.AddListener(OnLevelBegin);
        ServiceLocator.GetGameManager().OnLevelEnd.AddListener(OnLevelEnd);
        ServiceLocator.GetGameManager().winOnTimeOut = true;

        ServiceLocator.GetSoundSystem().PlaySound("hintVirgins");

        Vector3 rotation = flicker.rotation.eulerAngles;
        rotation.z = rotationAngle;
        targetRotation = Quaternion.Euler(rotation);
	}
	
	// Update is called once per frame
	void Update () {
        axisValue = Input.GetAxis("Horizontal");
        if (Mathf.Abs(prevAxisValue) < 0.3f && Mathf.Abs(axisValue) >= 0.3f) {
            Vector3 rotation = flicker.rotation.eulerAngles;
            rotation.z = -rotationAngle * Mathf.Sign(axisValue);
            targetRotation = Quaternion.Euler(rotation);

            ServiceLocator.GetSoundSystem().PlaySound("paddle");
        }
        prevAxisValue = axisValue;
    }

    void FixedUpdate() {
        float angVel = 0.0f;
        Quaternion newRotation = Util.smoothDampQuat(flicker.rotation, targetRotation, rotationTime, ref angVel);
        flicker.MoveRotation(newRotation);
    }

    void OnLevelBegin()
    {
        this.enabled = true;
    }

    void OnLevelEnd(bool won)
    {
        this.enabled = false;
    }
}
