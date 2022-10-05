using UnityEngine;
using System.Collections;

public class LaserWall : MonoBehaviour {
	
	public Transform respawn;

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == "Player_Ball") {
			other.transform.position = respawn.position;
		}
	}
}
