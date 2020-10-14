using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    public GameObject doorObject;
    public Vector3 destination;

    public float speed = 5f;
    public bool triggeredByPlayer = true;

    private Vector3 doorObjectStartingPosition;
    private bool open = false;
    public bool destinationReached = true;

	// Use this for initialization
	void Start () {
        doorObjectStartingPosition = doorObject.transform.localPosition;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (destinationReached)
            return;

        var currentDestination = destination;

        if (!open)
            currentDestination = doorObjectStartingPosition;

        //var newPosition = Vector3.Lerp(doorObject.transform.localPosition, currentDestination, speed * Time.fixedDeltaTime);

        var velocity = currentDestination - doorObject.transform.localPosition;

        doorObject.GetComponent<Rigidbody>().velocity = velocity * speed;

        if (Vector3.Distance(doorObject.transform.localPosition, currentDestination) < 0.01f)
        {
            //destinationReached = true;
        }
	}

    public void Open()
    {
        open = true;
        destinationReached = false;
    }

    public void Close()
    {
        open = false;
        destinationReached = false;
    }
}