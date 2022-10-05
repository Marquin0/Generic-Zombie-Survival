using UnityEngine;
using System.Collections;

public class ThrowerController : IsPlayerControlled {

	public GameObject ball;
	Rigidbody2D ballRigidbody2D;
	SpringJoint2D joint;
	bool firePressed = false;
	Vector2 prevVelocity;

	// Use this for initialization
	void Start () {
		ballRigidbody2D = ball.GetComponent<Rigidbody2D> ();
		joint = ball.GetComponent<SpringJoint2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (!firePressed && Input.GetAxis ("Fire1") == 1) {
			firePressed = true;
			ThrowBall ();
		} else if (Input.GetAxis ("Fire1") == 0) {
			firePressed = false;
		}
	}

	void Update() {
		if (joint.enabled) {
			if(Vector2.Distance(joint.connectedAnchor, ball.transform.position) < 1) {
				Destroy(joint);
				ballRigidbody2D.velocity = prevVelocity;
			}
			 else {
				prevVelocity = ballRigidbody2D.velocity;
			}
		}
	}

	void ThrowBall() {
		ball.transform.position = new Vector3(transform.position.x, transform.position.y+1, transform.position.z);
		ball.SetActive (true);
		joint.enabled = true;
		Vector3 vec3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		vec3.z = ball.transform.position.z;
		joint.connectedAnchor = vec3;


	}

	void OnEnable() {
		activateScripts (true);
	}
	
	override public void activateScripts (bool activate)
	{
		GetComponent<WalkController> ().enabled = activate;
		GetComponent<TransformToPlayer> ().enabled = activate;
	}
}
