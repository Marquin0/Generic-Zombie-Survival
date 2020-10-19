using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour {

    public bool teleportByTrigger = true;
    public bool onlyPlayer = false;
    public bool resetVelocity = false;
    public bool inlcudeOffset = false;
    public bool ignoreFirstTrigger = false;
    public int delay = 0;
    public float cooldown = 10f;
    public float lastTeleport = 0;
    public Transform teleportPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if(teleportByTrigger && (!onlyPlayer || other.tag == "Player"))
        {
            if(ignoreFirstTrigger)
            {
                ignoreFirstTrigger = false;
                return;
            }

            Teleport(other.gameObject);
        }
    }

    public void Teleport(GameObject gameObject)
    {
        if(lastTeleport <= 0) {
            StartCoroutine(TeleportObject(gameObject));
        }
    }

    private IEnumerator TeleportObject(GameObject other)
    {
        yield return new WaitForSeconds(delay);
        lastTeleport = cooldown;

        Vector3 offset = Vector3.zero;
        if (inlcudeOffset)
            offset = other.transform.position - transform.position;

        other.transform.position = teleportPosition.position + offset;

        if (resetVelocity)
        {
            var rigid = other.GetComponent<Rigidbody>();
            if (rigid != null)
            {
                rigid.velocity = Vector3.zero;
            }
        }

        var objectInteraction = other.gameObject.GetComponent<ObjectInteractions>();
        if (objectInteraction != null && objectInteraction.IsTelekinesed)
        {
            GameObject.FindGameObjectWithTag("Player").GetComponent<TelekinesisController>().DeselectGameObject(false);
        }
    }
}
