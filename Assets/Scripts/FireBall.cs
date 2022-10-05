using UnityEngine;
using System.Collections;

public class FireBall : MonoBehaviour {

	float speed = 0.3f;
	public GameObject explosion;
	public GameObject particles;

	// Use this for initialization
	void Start () {
		Destroy (gameObject, 10);
		if (transform.localScale.x < 0) {
			//gameObject.GetComponentInChildren<Transform>().localRotation = new Quaternion(0, -90, 0, 0);
			transform.GetChild(0).localRotation = Quaternion.Inverse(transform.GetChild(0).localRotation);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int direction = 1;
		if (transform.localScale.x < 0) {
			direction = -1;
		}

		transform.Translate (speed * direction, 0, 0);
	}

	void OnCollisionEnter2D(Collision2D other) {
		explosion.SetActive (true);
		GetComponent<SpriteRenderer> ().enabled = false;
		particles.GetComponent<ParticleSystem> ().Stop();
		Destroy (gameObject, 0.3f);
	}
}
