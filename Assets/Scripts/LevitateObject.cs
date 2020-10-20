using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitateObject : MonoBehaviour
{
	public float DegreesPerSecond = 15.0f;
	public float Amplitude = 0.5f;
	public float Frequency = 1f;
	public bool Rotate = false;
	
	private Vector3 posOffset = new Vector3();
	private Vector3 tempPos = new Vector3();
	
	void Start()
	{
		posOffset = transform.position;
	}
	
	void Update()
	{
		if(Rotate)
		{
			transform.Rotate(new Vector3(0f, Time.deltaTime * DegreesPerSecond, 0f), Space.World);
		}
		
		tempPos = posOffset;
		tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * Frequency) * Amplitude;

		transform.position = tempPos;
	}
}
