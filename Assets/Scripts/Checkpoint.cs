using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour {

	Transform respawn;

	void Start() {
		respawn = GameObject.FindGameObjectWithTag ("Respawn").transform;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag.Contains ("Player")) {
			respawn.position = transform.position;
		}
	}
}
