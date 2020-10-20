using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectController : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		ProjectileController projectileController = other.GetComponent<ProjectileController>();
		if(projectileController != null)
		{
			if(projectileController.owner != null)
			{
				projectileController.velocity = (projectileController.owner.transform.position - other.transform.position).normalized;
			}
			else
			{
				projectileController.velocity *= -1;
			}
			other.transform.rotation = Quaternion.LookRotation(projectileController.velocity);
			projectileController.owner = null;
		}
	}
}
