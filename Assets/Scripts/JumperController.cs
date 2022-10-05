using UnityEngine;
using System.Collections;

public class JumperController : IsPlayerControlled {

	Animator anim;
	WalkController walkController;
	private Rigidbody2D rigidbody2D;

	public bool walled = false;
	public Transform leftWallCheck;
	public Transform rightWallCheck;
	float groundRadius = 0.1f;
	public LayerMask whatAreWalls;
	bool rush = false;
	bool rushing = false;
	float speed = 0.6f;
	float rushingTime = 0f;
	float rushingLength = 1f;
	float xDirection = 0, yDirection = 0;
	bool firePressed = false;

	// Use this for initialization
	void Start () {
		walkController = GetComponent<WalkController> ();
		anim = GetComponent<Animator> ();
		rigidbody2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		walled = false;
		walled = Physics2D.OverlapCircle (leftWallCheck.position, groundRadius, whatAreWalls);
		walled = walled || Physics2D.OverlapCircle (rightWallCheck.position, groundRadius, whatAreWalls);
		anim.SetBool ("Walled", walled);

		if (rushing) {
			transform.Translate(speed*xDirection, speed*yDirection, 0);
			rushingTime += 0.1f;
			if(rushingLength <= rushingTime) {
				rushing = false;
				walkController.enabled = true;
				rigidbody2D.gravityScale = 1;
			}
		}
	}

	void Update() {
		if (!firePressed && Input.GetAxis ("Fire1") == 1 && !rushing && !rush) {
			firePressed = true;
			rush = true;
			rushingTime = 0f;
			walkController.enabled = false;
			rigidbody2D.gravityScale = 0;
			rigidbody2D.velocity = new Vector2 (0, 0);
		} else if (Input.GetAxis ("Fire1") == 0 && !rushing && !rush) {
			firePressed = false;
		}
		if(rush) {
			xDirection = Input.GetAxisRaw ("Horizontal");
			yDirection = Input.GetAxisRaw ("Vertical");
			if(xDirection < -0.3) xDirection = -1;
			else if(xDirection > 0.3) xDirection = 1;
			else xDirection = 0;
			if(yDirection < -0.3) yDirection = -1;
			else if(yDirection > 0.3) yDirection = 1;
			else yDirection = 0;

			if(xDirection != 0 || yDirection != 0) {
				rushing = true;
				rush = false;
			}
		}
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
