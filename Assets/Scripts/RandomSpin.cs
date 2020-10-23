using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpin : MonoBehaviour
{
	private Vector3 randomDirection;

    // Start is called before the first frame update
    void Start()
    {
		randomDirection = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
		transform.Rotate(randomDirection.x, randomDirection.y, randomDirection.z);
    }
}
