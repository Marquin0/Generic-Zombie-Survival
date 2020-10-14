using UnityEngine;
using System.Collections;

public class CameraStalker : MonoBehaviour {

	public GameObject player;
	public GameObject shadowBall;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (!player.activeSelf) {
			GameObject obj = GameObject.FindGameObjectWithTag("Player");
			if(obj != null) player = obj;
		}

		Transform trans = player.transform;
		if (shadowBall.activeSelf) {
			trans = shadowBall.transform;
		}
		transform.position = new Vector3 (trans.transform.position.x, trans.transform.position.y, transform.position.z);
	}
}
