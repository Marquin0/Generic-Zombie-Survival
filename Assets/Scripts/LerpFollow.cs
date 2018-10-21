using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpFollow : MonoBehaviour {

    public Transform target;
    public float speed = 5f;
    public bool keepOffset = false;

    private Vector3 offset;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        offset = target.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate () {
        var newPosition = Vector3.Lerp(transform.position, target.position, speed * Time.fixedDeltaTime);
        if (keepOffset)
            newPosition += offset;

        transform.position = newPosition;
	}
}
