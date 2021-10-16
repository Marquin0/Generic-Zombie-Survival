using UnityEngine;
using System.Collections;

public class TransformToPlayer : MonoBehaviour {

	public GameObject player;
	string tag;
	public bool renameTagToPlayer = true;

	// Use this for initialization
	void Start () {
		tag = gameObject.tag;
	}

	void OnEnable() {
		if (renameTagToPlayer) {
			gameObject.tag = "Player";
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Fire2") == 1) {
			TransformTOPlayer();
		}
	}

	public void TransformTOPlayer() {
		player.transform.position = transform.position;
		player.SetActive(true);
		gameObject.SetActive(false);
	}
}
