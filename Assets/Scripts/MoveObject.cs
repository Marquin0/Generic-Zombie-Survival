using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour {

    public Transform gameObjectToMove;
    public Vector3 destination;
    public bool triggeredByPlayer;
    public float speed = 1f;

    private bool moveToDestination = false;
    private bool moveToStartingPosition = false;
    private Vector3 startingPosition;

	// Use this for initialization
	void Start () {
        startingPosition = transform.position;
        
    }

    // Update is called once per frame
    void FixedUpdate () {
        Vector3 dest;

        if (moveToDestination)
        {
            dest = destination;
        }
        else if (moveToStartingPosition)
        {
            dest = startingPosition;
        }
        else
            return;

        Transform objectToMove = gameObjectToMove;
        if (objectToMove == null)
            objectToMove = gameObject.transform;

        var nextPos = Vector3.Lerp(objectToMove.position, dest, speed * Time.fixedDeltaTime);

        objectToMove.position = nextPos;

        if (Vector3.Distance(objectToMove.position, destination) <= 0.1f)
        {
            StopMovement();
        }
    }

    public void MoveToDestination()
    {
        moveToDestination = true;
        moveToStartingPosition = false;
    }

    public void MoveToStartingPosition()
    {
        moveToStartingPosition = true;
        moveToDestination = false;
    }

    public void StopMovement()
    {
        moveToDestination = false;
        moveToStartingPosition = false;
    }
}
