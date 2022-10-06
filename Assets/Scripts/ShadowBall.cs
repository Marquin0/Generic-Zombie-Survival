using UnityEngine;
using System.Collections;

public class ShadowBall : MonoBehaviour, HasHealthInterface {

	Animator anim;
	float mouseSpeed = 0.25f;
	float speed = 10f;
	float maxLifeTime = 5f;
	float currentLifeTime;
	Rigidbody2D rigidbody2D;

	private TransformToPlayer transScript;

	// Use this for initialization
	void Start () {	
		anim = GetComponent<Animator> ();
		rigidbody2D = GetComponent<Rigidbody2D> ();
		transScript = GetComponent<TransformToPlayer> ();
	}

	void OnEnable() {
		currentLifeTime = maxLifeTime;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		float hMove = Input.GetAxis ("Horizontal");
		float vMove = Input.GetAxis ("Vertical");

		if(hMove != 0 || vMove != 0) {
			rigidbody2D.velocity = new Vector2(speed * hMove, speed * vMove);
		}
//		else if (Input.GetAxis ("Fire1") == 1) {
//			Vector3 vec3 = Camera.main.ScreenToWorldPoint (Input.mousePosition);
//			vec3.z = transform.position.z;
//			float distance = Vector3.Distance(transform.position, vec3);
//
//			if(distance>0){
//				transform.position = Vector3.Lerp (
//					transform.position, vec3, mouseSpeed/distance);
//			}
//
//		}
	}

	void Update() {
		currentLifeTime -= Time.deltaTime;

		if (currentLifeTime < 0) {
			transScript.TransformTOPlayer();
		}
//		if (Input.GetAxis ("Fire2") == 1) {
//			transScript.TransformTOPlayer();
//		}
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "Player_Controllable") {

			other.gameObject.GetComponent<IsPlayerControlled>().enabled = true;
			gameObject.SetActive(false);
		}
	}

	public float getMaxHealth ()
	{
		return maxLifeTime;
	}

	public float getCurrentHealth ()
	{
		return currentLifeTime;
	}
}
