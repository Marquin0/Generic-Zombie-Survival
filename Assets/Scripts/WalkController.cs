using UnityEngine;
using System.Collections;

public class WalkController : MonoBehaviour {

	public float maxSpeed = 7.5f;
	public bool facingRight = true;
	private Rigidbody2D rigidbody2D;
	public bool canDoubleJump = true;
	public bool doubleJump = false;
	public int jumpForce = 700;
	
	public bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.1f;
	public LayerMask whatIsGround;
	
	Animator anim;
	bool jumpPressed = false;

	Healthbar healthBarScript;

	// Use this for initialization
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
		healthBarScript = GetComponentInChildren<Healthbar> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);
		
		if (grounded) {
			doubleJump = false;
		}
		
		anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		
		float move = Input.GetAxis ("Horizontal");
		
		anim.SetFloat ("Speed", Mathf.Abs (move));

		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		
		if (move > 0 && !facingRight) {
			Flip ();
		} else if (move < 0 && facingRight) {
			Flip ();
		}
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		if (healthBarScript != null) {
			healthBarScript.Flip();
		}
	}

	void Update() {
		if ((grounded || (!doubleJump && canDoubleJump)) && Input.GetAxis("Jump") == 1 && !jumpPressed) {
			jumpPressed = true;
			if (!grounded) {
				doubleJump = true;
			}
			anim.SetBool ("Ground", false);
			rigidbody2D.velocity = Vector2.zero;
			rigidbody2D.angularVelocity = 0f;
			rigidbody2D.AddForce (new Vector2 (0, jumpForce));
		} else if (Input.GetAxis ("Jump") == 0) {
			jumpPressed = false;
		}
	}
}
