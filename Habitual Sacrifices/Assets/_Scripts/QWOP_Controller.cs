using UnityEngine;
using System.Collections;

public class QWOP_Controller : MonoBehaviour
{
    public GameObject leftArm;
    public GameObject rightArm;
    public GameObject paste;

    public float speedVar = 1f;
    public float smoothTime = 2f;
    
    private Vector3 rightHandPosition;
    private Vector3 rightHandTarget;
    private Vector3 leftHandPosition;
    private Vector3 leftHandTarget;

    private Vector3 rightVelocity = Vector3.zero;
    private Vector3 leftVelocity = Vector3.zero;

	// Use this for initialization
	void Start () {
        this.enabled = false;
        ServiceLocator.GetGameManager().OnLevelBegin.AddListener(OnLevelBegin);
        ServiceLocator.GetGameManager().OnLevelEnd.AddListener(OnLevelEnd);

        ServiceLocator.GetSoundSystem().PlaySound("hintVirgins"); //play frustrated grunts & no BGM

        rightHandPosition = rightArm.transform.position;
        leftHandPosition = leftArm.transform.position;

        rightHandTarget = rightHandPosition;
        leftHandTarget = leftHandPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        int rightPosX = Input.GetButton("RightPosX") ? 1 : 0;
        int rightNegX = Input.GetButton("RightNegX") ? -1 : 0;
        int rightPosY = Input.GetButton("RightPosY") ? 1 : 0;
        int rightNegY = Input.GetButton("RightNegY") ? -1 : 0;

        int leftPosX = Input.GetButton("LeftPosX") ? 1 : 0;
        int leftNegX = Input.GetButton("LeftNegX") ? -1 : 0;
        int leftPosY = Input.GetButton("LeftPosY") ? 1 : 0;
        int leftNegY = Input.GetButton("LeftNegY") ? -1 : 0;

        float leftX = (leftNegX + leftPosX);// * speedVar;
        float leftY = (leftNegY + leftPosY);// * speedVar;
        float rightX = (rightNegX + rightPosX);// * speedVar;
        float rightY = (rightNegY + rightPosY);// * speedVar;

        rightHandTarget = new Vector3(rightHandPosition.x += rightX, rightHandPosition.y += rightY, rightHandPosition.z);
        leftHandTarget = new Vector3(leftHandPosition.x += leftX, leftHandPosition.y += leftY, leftHandPosition.z);

        rightArm.transform.position = Vector3.SmoothDamp(rightHandPosition, rightHandTarget, ref rightVelocity, smoothTime);
        leftArm.transform.position = Vector3.SmoothDamp(leftHandPosition, leftHandTarget, ref leftVelocity, smoothTime);

        //Debug.Log(string.Format("Inputs -- Left X: {0},{1} LeftY: {2},{3} RightX: {4},{5} RightY: {6},{7}", leftNegX, leftPosX, leftNegY, leftPosY, rightNegX, rightPosX, rightNegY, rightPosY));
        //Debug.Log("Right Arm Position: " + rightArm.transform.position.ToString());
        //Debug.Log(string.Format("Right Arm Target: {0}", rightHandTarget.ToString()));
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
