using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SwitchTrigger : MonoBehaviour {

    public bool triggeredByPlayer = true;
    public bool onlyByObject = false;
    public bool triggeredByKey = false;
    public KeyCode triggerKey = KeyCode.None;
    public float dissableKeyFor = 1.5f;
    public bool triggeredByView = false;
    public GameObject triggerView;
    public UnityEvent events;
    public UnityEvent exitEvents;

    private bool playerEntered = false;
    private float keyLastPushed = 0;
    private Renderer triggerViewRenderer;

    private void Start()
    {
        if (triggerView != null)
            triggerViewRenderer = triggerView.GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if((onlyByObject && other.tag != "Player") || !onlyByObject && (!triggeredByPlayer || other.tag == "Player"))
        {
            if (other.tag == "Player")
                playerEntered = true;

            if(!triggeredByKey && !triggeredByView)
                events.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(!triggeredByPlayer || other.tag == "Player")
        {
            if (other.tag == "Player")
                playerEntered = false;

            exitEvents.Invoke();
        }
    }

    private void Update()
    {
        keyLastPushed += Time.deltaTime;

        if (triggeredByView && triggerViewRenderer.isVisible)
        {
            events.Invoke();
        }

        if (!playerEntered)
            return;
        
        if(Input.GetKeyDown(triggerKey) && keyLastPushed >= dissableKeyFor)
        {
            keyLastPushed = 0f;
            events.Invoke();
        }
    }
}