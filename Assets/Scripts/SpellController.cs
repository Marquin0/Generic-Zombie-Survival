using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpellController : MonoBehaviour
{
	internal Spell spell;
	internal Rigidbody rb;
	internal GameObject owner;
	internal Vector3 velocity;
	internal bool casted = false;

	void Start()
	{
		spell = GetComponent<Spell>();
		rb = GetComponent<Rigidbody>();
	}

	public virtual void CastSpell(GameObject owner, Vector3 velocity)
	{
		this.owner = owner;
		this.velocity = velocity;
		casted = true;
	}
}
