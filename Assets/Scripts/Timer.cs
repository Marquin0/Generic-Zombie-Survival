using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour {

    public float delayInSeconds = 10f;
    public bool loop = false;
    public bool StartAutomatically = false;
    public bool disableTimerWhenFinished = true;

    public UnityEvent events;

    private bool running = false;
    private float pastTime = 0;

    public void StartTimer()
    {
        pastTime = 0;
        running = true;
    }

    public void StopTimer()
    {
        running = false;
    }

	// Use this for initialization
	void Start () {
        if (StartAutomatically)
            StartTimer();
	}
	
	// Update is called once per frame
	void Update () {
        if (!running)
            return;

        pastTime += Time.deltaTime;

        if(pastTime >= delayInSeconds)
        {
            events.Invoke();
            StopTimer();

            if (loop)
            {
                StartTimer();
                return;
            }
            if (disableTimerWhenFinished)
                enabled = false;
        }
	}
}
