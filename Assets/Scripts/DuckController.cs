using UnityEngine;
using System.Collections;

public class DuckController : IsPlayerControlled {

	public GameObject fireballPrefab;
	GameObject fireball;

	// Use this for initialization
	void Start () {

	}

	void OnEnable() {
		activateScripts (true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis ("Fire1") == 1 && fireball == null) {
			fireball = Instantiate(fireballPrefab);
			int direction = 1;
			if(transform.localScale.x < 0) {
				direction = -1;
			}
			fireball.transform.localScale = new Vector3(fireball.transform.localScale.x * direction, fireball.transform.localScale.y, fireball.transform.localScale.z);
			fireball.transform.localPosition = new Vector3(transform.position.x + (1.2f * direction), transform.position.y, fireball.transform.localPosition.z);
			fireball.SetActive(true);
		}
	}

	override public void activateScripts (bool activate)
	{
		GetComponent<WalkController> ().enabled = activate;
		GetComponent<TransformToPlayer> ().enabled = activate;
	}
}
