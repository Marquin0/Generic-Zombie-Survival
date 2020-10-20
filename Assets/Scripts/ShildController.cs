using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShildController : MonoBehaviour
{
	public GameObject Shild;

	private Player player;

    // Start is called before the first frame update
    void Start()
    {
		player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player.ShildCooldown > 0)
		{
			player.ShildCooldown -= Time.deltaTime;
			return;
		}

		if(Input.GetKeyDown(KeyCode.F) && IsShildReady())
		{
			ActivateShild();
		}

		if(player.IsShildActive)
		{
			player.ShildCurrentActiveTime -= Time.deltaTime;
			if(player.ShildCurrentActiveTime <= 0)
			{
				DeactivateShild();
			}
		}
    }

	private bool IsShildReady()
	{
		return player.ShildCooldown <= 0 && !player.IsShildActive;
	}

	private void ActivateShild()
	{
		player.IsShildActive = true;
		player.ShildCurrentActiveTime = player.ShildActiveTime;
		Shild.SetActive(true);
	}
	private void DeactivateShild()
	{
		player.IsShildActive = false;
		player.ShildCooldown = player.ShildMaximumCooldown;
		Shild.SetActive(false);
	}
}
