using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindController : MonoBehaviour {

    private bool rewinding = false;
    public bool onlyRewindVisibleObjects = false;
    public GameObject orbEffect;

	// Use this for initialization
	void Start () {
		
	}

    private void OnEnable()
    {
        orbEffect.SetActive(true);
    }

    private void OnDisable()
    {
        orbEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            if (rewinding)
                return;
            rewinding = true;
            foreach (var rewindObject in Rewind.RewindObjects)
            {
                if (!onlyRewindVisibleObjects || rewindObject.gameObject.GetComponent<Renderer>().isVisible)
                    rewindObject.StartRewind();
            }
        }
        else if (rewinding)
        {
            rewinding = false;
            foreach (var rewindObject in Rewind.RewindObjects)
            {
                rewindObject.StopRewind();
            }
        }
    }
}
