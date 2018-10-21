using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MineController : NetworkBehaviour {

    float miningSpeed = 2f;
    float miningDelay = 0f;
    int miningAmount = 10;
    
    PlayerBuilder builderScript;

	// Use this for initialization
	void Start () {
        builderScript = GetComponent<Building>().playerBuilderScript;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;

        miningDelay -= Time.deltaTime;

        if(miningDelay <= 0)
        {
            miningDelay = miningSpeed;

            builderScript.RpcIncreaseGoldResource(miningAmount);
        }
	}
}
