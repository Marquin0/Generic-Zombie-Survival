using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightFlicker : MonoBehaviour {

    public float minimumDelay = 0.5f, maximumDelay = 5f;
    public float offLength = 0.2f;

    public AudioClip[] clips;

    private float lastFlicker, nextFlicker;

    private Light light;
    private AudioSource audioSource;

	// Use this for initialization
	void Start () {
        light = GetComponent<Light>();
        audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
        lastFlicker += Time.deltaTime;

        if(lastFlicker >= nextFlicker)
        {
            lastFlicker = 0f;
            nextFlicker = Random.Range(minimumDelay, maximumDelay);

            light.enabled = false;

            audioSource.clip = clips[Random.Range(0, clips.Length)];
            audioSource.Play();

            Invoke("TurnOn", offLength);
        }
	}

    private void TurnOn()
    {
        light.enabled = true;
    }
}
