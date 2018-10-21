using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportController : MonoBehaviour {

    public GameObject teleportLocationPrefab;
    public float maxDistance = 15f;
    public float delayTillNextTeleport = 1f;
    public bool teleportEverywhere = false;
    public GameObject orbEffect;

    private float nextTeleportIn;
    private bool leftClicking = false;
    private GameObject teleportLocation;
    private Vector3? oldPosition;

    public Image crosshair;

    // Use this for initialization
    void Start () {
        if (teleportLocation == null)
            teleportLocation = Instantiate(teleportLocationPrefab);
    }

    private void OnEnable()
    {
        orbEffect.SetActive(true);
    }

    private void OnDisable()
    {
        if (teleportLocation != null)
            teleportLocation.SetActive(false);
        crosshair.color = Color.white;
        leftClicking = false;
        orbEffect.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        nextTeleportIn -= Time.deltaTime;
        if (nextTeleportIn > 0)
        {
            crosshair.color = Color.white;
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            leftClicking = true;
            
            teleportLocation.SetActive(true);
        }
        if (Input.GetMouseButtonUp(0))
        {
            leftClicking = false;
            if (teleportLocation != null && teleportLocation.activeSelf)
            {
                oldPosition = transform.position;
                transform.position = teleportLocation.transform.position + new Vector3(0, .5f, 0);
                nextTeleportIn = delayTillNextTeleport;
            }
            teleportLocation.SetActive(false);
        }
        if(Input.GetMouseButtonDown(1) && oldPosition.HasValue)
        {
            transform.position = oldPosition.Value;
            oldPosition = null;
            nextTeleportIn = delayTillNextTeleport;
        }

        RaycastHit raycastHit;
        bool locationFound = false;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out raycastHit, maxDistance))
        {
            var objectInteractions = raycastHit.collider.gameObject.GetComponent<ObjectInteractions>();
            if (objectInteractions != null && objectInteractions.CanBeTeleportedOn || teleportEverywhere)
            {
                crosshair.color = new Color32(57, 177, 236, 255);
                locationFound = true;
            }
            else
            {
                teleportLocation.SetActive(false);
                crosshair.color = Color.white;
            }
        }
        else
        {
            teleportLocation.SetActive(false);
            crosshair.color = Color.white;
        }

        if (teleportLocation != null && leftClicking && locationFound)
        {
            teleportLocation.transform.position = raycastHit.point - ray.direction;
            teleportLocation.transform.rotation = raycastHit.transform.rotation;
            teleportLocation.transform.rotation *= Quaternion.Euler(-90, 0, 0);
            teleportLocation.SetActive(true);
        }
    }
}
