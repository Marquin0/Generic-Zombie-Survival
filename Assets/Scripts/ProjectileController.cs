using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ProjectileController : SpellController
{
	private float traveled = 0;
	private bool sticking = false;
	private float stickTimer = 0;

	public bool StickInTarget;

	public float StickTimeInSeconds;

	public override void CastSpell(GameObject owner, Vector3 velocity)
	{
		base.CastSpell(owner, velocity);


	}

	private void Update()
	{
		if(sticking)
		{
			stickTimer += Time.deltaTime;
			if(stickTimer >= StickTimeInSeconds)
			{
				Destroy(gameObject);
			}
		}
	}

	// Update is called once per frame
	void FixedUpdate()
    {
        if(velocity == null || sticking)
		{
			return;
		}

		Vector3 newPos = velocity * spell.Speed * Time.fixedDeltaTime;
		traveled += newPos.magnitude;

		rb.MovePosition(transform.position + newPos);

		if(traveled >= spell.Range)
		{
			Destroy(gameObject);
		}
    }

	//[Server]
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject == owner || !casted || other.tag == "Shild" || other.tag == "Spell" || other.GetComponent<Player>()?.IsShildActive == true)
		{
			return;
		}

		PlayerController hitPlayerController = other.GetComponent<PlayerController>();
		if(hitPlayerController != null)
		{
			hitPlayerController.ApplySpellHit(spell);
			spell.HitPlayer = other.GetComponent<Player>();
		}

		spell.CastSpellEffects(new HitParameter { Position = transform.position, Velocity = velocity });
		spell.PlaySoundEffect(SpellSoundEffect.Hit, transform.position);

		if (StickInTarget)
		{
			sticking = true;
			transform.parent = other.transform;
			transform.localScale *= 0.5f;
			transform.position -= velocity / 2;
			Destroy(GetComponent<Collider>());
			Destroy(GetComponent<Rigidbody>());
		}
		else
		{
			Destroy(gameObject);
		}
	}

	//[ClientRpc]
	private void RpcDoThisOnClient(Vector3 v)
	{

	}

	public void SetupProjectile(Vector3 velocity, GameObject owner)
	{
		this.velocity = velocity.normalized;
		this.owner = owner;
	}
}
