using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rewind : MonoBehaviour {

    public static List<Rewind> RewindObjects = new List<Rewind>();

    private List<PointInTime> pointsInTime = new List<PointInTime>();
    public static float MaxRewind = 50f;
    public bool rewinding = false;
    private Rigidbody rigidbody;
    private Vector3 lastVelocity;

    private bool isKinematic = false;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody>();
	}

    private void OnEnable()
    {
        RewindObjects.Add(this);
    }

    private void OnDisable()
    {
        RewindObjects.Remove(this);
    }

    private void Update()
    {  
    }

    // Update is called once per frame
    void FixedUpdate () {
        Record();
        Rewinding();
	}

    private void Rewinding()
    {
        if (!rewinding)
            return;

        if (pointsInTime.Count > 0)
        {
            var pastPoint = pointsInTime[0];
            pointsInTime.RemoveAt(0);

            transform.position = pastPoint.Position;
            transform.rotation = pastPoint.Rotation;
            lastVelocity = pastPoint.Velocity;
            rigidbody.isKinematic = pastPoint.IsKinematic;
        }
        else
        {
            StopRewind();
        }
    }

    public void StartRewind()
    {
        rewinding = true;
        isKinematic = rigidbody.isKinematic;
        rigidbody.isKinematic = true;
    }

    public void StopRewind()
    {
        rewinding = false;
        rigidbody.isKinematic = isKinematic;
        rigidbody.velocity = lastVelocity;
    }

    private void Record()
    {
        if (rewinding)
            return;

        if (pointsInTime.Count == 0 || (pointsInTime.Count > 0 && (transform.position != pointsInTime[0].Position || transform.rotation != pointsInTime[0].Rotation || rigidbody.isKinematic != pointsInTime[0].IsKinematic)))
        {
            if (pointsInTime.Count >= MaxRewind / Time.fixedDeltaTime)
            {
                pointsInTime.RemoveAt(pointsInTime.Count - 1);
            }
            pointsInTime.Insert(0, new PointInTime(transform.position, transform.rotation, rigidbody.velocity, rigidbody.isKinematic));
        }
    }
}

public class PointInTime
{
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Vector3 Velocity { get; set; }
    public bool IsKinematic { get; set; }

    public PointInTime(Vector3 position, Quaternion rotation, Vector3 velocity, bool isKinematic)
    {
        Position = position;
        Rotation = rotation;
        Velocity = velocity;
        IsKinematic = isKinematic;
    }
}