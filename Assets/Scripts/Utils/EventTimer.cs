using UnityEngine;

public class EventTimer
{
    private float currentTime = 0;
    private bool running = false;

    public float intervalInSeconds { get; private set; }

    public delegate void TickEventHandler();

    public event TickEventHandler Tick;

    public EventTimer(float intervalInSeconds)
    {
        this.intervalInSeconds = intervalInSeconds;
        currentTime = intervalInSeconds;
    }

    public void Start()
    {
        running = true;
    }

    public void Stop()
    {
        running = false;
    }

    public void Update()
    {
        if (!running) { return; }

        currentTime -= Time.deltaTime;
        if (currentTime < 0)
        {
            currentTime = intervalInSeconds;
            if (Tick != null) Tick.Invoke();
        }
    }
}
