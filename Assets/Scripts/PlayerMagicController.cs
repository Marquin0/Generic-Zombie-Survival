using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerMagicController : NetworkBehaviour
{
	private Player player;
	private int selectedSpell = 0;
	private bool chargingSpell;
	private float distanceToGround;
	private float currentlySpendMana = 0;

	private GameObject instantiatedSpellPrefab;
	private Spell instantiatedSpell;
	private GameObject shrinkingObject;

	public GameObject[] SpellPrefabs;
	public UIBarController ManaBarController;
	public float ShrinkSpeed = 15f;

	// Start is called before the first frame update
	void Start()
    {
		player = GetComponent<Player>();
		distanceToGround = GetComponent<Collider>().bounds.extents.y;
	}

	// Update is called once per frame
	void Update()
    {
		if(isLocalPlayer)

		UpdateMana();
		UpdateManaBar();
		ShrinkSpell();
	}

	private void UpdateMana()
	{
		if(chargingSpell)
		{
			float manaCost = instantiatedSpell.GetChargingManaCost();
			if (player.CurrentMana < manaCost)
			{
				instantiatedSpell.StopCharging();
				return;
			}

			currentlySpendMana += manaCost;
			player.CurrentMana -= manaCost;
			return;
		}

		player.UpdateMana();
	}

	public void Activate(Vector3 displayPoint, Vector3 spawnPoint, Vector3 velocity)
	{
		if(chargingSpell)
		{
			return;
		}

		currentlySpendMana = 0;
		chargingSpell = true;
		instantiatedSpellPrefab = Instantiate(SpellPrefabs[selectedSpell], displayPoint, Quaternion.LookRotation(velocity), transform);

		instantiatedSpell = instantiatedSpellPrefab.GetComponent<Spell>();
		instantiatedSpellPrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;

		if(player.CurrentMana < instantiatedSpell.GetInitialManaCost())
		{
			chargingSpell = false;
			Destroy(instantiatedSpellPrefab);
			return;
		}

		player.CurrentMana -= instantiatedSpell.GetInitialManaCost();

		if (instantiatedSpell.IsInstantSpell())
		{
			player.CurrentMana -= instantiatedSpell.ManaCost;
			ActivateSpellController(spawnPoint, velocity);
			chargingSpell = false;
		}

		if(chargingSpell)
		{
			player.SlowTimeInAir = true;
			instantiatedSpell.PlaySoundEffect(SpellSoundEffect.Charging, displayPoint, transform);
		}
	}

	private void ActivateSpellController(Vector3 spawnPoint, Vector3 velocity)
	{
		if(instantiatedSpellPrefab == null)
		{
			return;
		}

		instantiatedSpell.StopSoundEffect(SpellSoundEffect.Charging);
		instantiatedSpell.PlaySoundEffect(SpellSoundEffect.Fire, transform.position);
		instantiatedSpell.StopCharging();
		SpellController spellController = instantiatedSpellPrefab.GetComponent<SpellController>();
		spellController.CastSpell(gameObject, velocity);

		instantiatedSpellPrefab.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
		instantiatedSpellPrefab.transform.parent = null;
		instantiatedSpellPrefab.transform.position = spawnPoint;
		instantiatedSpellPrefab.transform.rotation = Quaternion.LookRotation(velocity);
	}

	public void Deactivate(Vector3 spawnPoint, Vector3 velocity)
	{
		player.SlowTimeInAir = false;
		chargingSpell = false;
		if(instantiatedSpellPrefab == null)
		{
			return;
		}

		if(!instantiatedSpell.IsReady())
		{
			instantiatedSpell.StopSoundEffect(SpellSoundEffect.Charging);
			RevertMagicCharging(instantiatedSpellPrefab);
			return;
		}
		
		if(instantiatedSpell.IsFullyCharged())
		{
			player.CurrentMana += instantiatedSpell.ManaGainWhenFullyCharged;
		}
		ActivateSpellController(spawnPoint, velocity);
	}

	public void ChangeSelectedSpell(int index)
	{
		selectedSpell = index;
	}

	private void UpdateManaBar()
	{
		ManaBarController.SetBarPercentage(player.CurrentMana / player.MaximumMana);
	}

	private bool IsGrounded()
	{
		return Physics.Raycast(transform.position, -Vector3.up, distanceToGround + 0.1f);
	}

	private void RevertMagicCharging(GameObject o)
	{
		player.CurrentMana += currentlySpendMana;
		shrinkingObject = o;
		instantiatedSpell?.StopCharging();
		instantiatedSpell = null;
		instantiatedSpellPrefab = null;
	}

	private void ShrinkSpell()
	{
		if(shrinkingObject != null)
		{
			shrinkingObject.transform.localScale -= shrinkingObject.transform.localScale *  Time.deltaTime * ShrinkSpeed;
			if(shrinkingObject.transform.localScale.x <= 0.1)
			{
				Destroy(shrinkingObject);
				shrinkingObject = null;
			}
		}
	}
}