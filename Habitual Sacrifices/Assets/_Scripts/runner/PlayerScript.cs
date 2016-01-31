using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

	private Vector3 maxVelocity = new Vector3 (5, 7,10);
	//public Vector3 robotPos;
	public Transform robotPos;
    public Transform teleport;

	public float speedX = 1.0f;
	public float speedFwd = 1.0f;
	public float maxSpeedUp = 3.0f;

	private int jumpCount = 0;
	private bool onGround = false;
    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        ServiceLocator.GetGameManager().winOnTimeOut = true;

		PhysicMaterial physicMat = new PhysicMaterial();
		GetComponent<Collider>().material = physicMat;
		physicMat.bounciness = 0;
		physicMat.dynamicFriction = 0.2f;
        ServiceLocator.GetSoundSystem().PlayBackgroundMusic("runner");
        rb = GetComponent<Rigidbody>();
    }

	void OnCollisionStay(Collision other) 
	{
        if (other.gameObject.tag == "Platform")
        {
            onGround = true;
        }
	}

	void OnCollisionExit(Collision other) 
	{
		if (other.gameObject.tag == "Platform")
		{
			onGround = false;
		}
	}

	void FixedUpdate()
	{
        float speedFactor = ServiceLocator.GetGameManager().GetSpeedFactor(maxSpeedUp);
        float axisValue = Input.GetAxis("Horizontal");

		transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z + speedFwd * speedFactor);
		
		if (gameObject.transform.position.y <= -10.0f) {
            LoseGame();
        }

		float absVelocityY = Mathf.Abs(rb.velocity.y);

        if (Mathf.Abs(axisValue) > 0.3f)
        {
            rb.AddForce(Mathf.Sign(axisValue) * speedX, 0, 0);
        }

		if (Input.GetButtonDown("Action") && onGround == true)
        {
            GetComponent<Rigidbody>().AddForce(0,300,0);
        }

        if (gameObject.transform.position.y >= 10 &&  absVelocityY >= maxVelocity.y  )  // fixes glitch where player flys up into air.
        {
            gameObject.transform.position = teleport.transform.position;
            rb.velocity = new Vector3(rb.velocity.x/2, 0, rb.velocity.y/2);
        }
	}

    void LoseGame() {
        ServiceLocator.GetGameManager().LoseLife();
        ServiceLocator.GetSoundSystem().PlaySound("badSound");
    }

	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "Obstacle")
		{
            LoseGame();
		}
	}
}
