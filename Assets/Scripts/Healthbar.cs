using UnityEngine;
using System.Collections;

public class Healthbar : MonoBehaviour {

	HasHealthInterface gameObject;
	float maxSize;

	// Use this for initialization
	void Start () {
		gameObject = GetComponentInParent<HasHealthInterface> ();
		maxSize = transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		transform.localScale = new Vector3 (gameObject.getCurrentHealth() / gameObject.getMaxHealth() * maxSize, transform.localScale.y, transform.localScale.z);
	}

	public void Flip() {
		Vector3 theScale = transform.localPosition;
		theScale.x *= -1;
		transform.localPosition = theScale;
	}
}
