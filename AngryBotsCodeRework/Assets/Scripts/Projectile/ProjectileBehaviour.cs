using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileBehaviour : MonoBehaviour {

	[Header("Movement")]
	public float speed;
	private Rigidbody projectileRigidbody;

	[Header("Life Settings")]
	public float lifeTime;

	[Header("Damage")]
	public int damageToEnemy;

	[Header("Hit Object AVFX")]
	public BulletTypes hitEnemy;
	public BulletTypes hitWall;

	void OnEnable()
	{
		projectileRigidbody = GetComponent<Rigidbody>();
		lifeTime = 2f;
	}
	
	void OnTriggerEnter(Collider theCollider)
	{
		if(theCollider.CompareTag("Enemy"))
		{
			if(damageToEnemy > 0)
			{
				theCollider.GetComponent<EnemyHealth>().RemoveHealth(damageToEnemy);	
			}

			BulletsPool.current.Take(hitEnemy, transform.position, transform.rotation);
			RemoveProjectile();

		} else if (theCollider.CompareTag("Environment"))
		{
			BulletsPool.current.Take(hitWall, transform.position, transform.rotation);
			RemoveProjectile();
		}

	}
	
	void Update()
	{
		lifeTime -= Time.deltaTime;
		
		Vector3 movement = transform.forward * speed * Time.deltaTime;
		projectileRigidbody.MovePosition(transform.position + movement);
		if (lifeTime < 0)
			RemoveProjectile();
	}

	void RemoveProjectile()
	{
		gameObject.SetActive(false);
	}

}
