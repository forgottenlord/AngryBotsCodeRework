using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ObjectPooling;

public class PlayerProjectileSpawner : MonoBehaviour {


	[Header("Input")]
	public KeyCode spawnKey = KeyCode.Mouse0;


	[Header("Spawner Settings")]
	[SerializeField] public BulletTypes bulletType;
	public Transform spawnPoint;

	[SerializeField] private float spawnRate;
	[SerializeField] private float timer;
	[SerializeField] private float reloadTime;
	[SerializeField] private byte maxAmmo;
	
	[Header("Particles")]
	public ParticleSystem spawnParticles;
	
	[Header("Audio")]
	public AudioSource spawnAudioSource;

	[SerializeField] private byte remainAmmo;
	[SerializeField] private bool isReloading = false;
    private void Start()
    {
		remainAmmo = maxAmmo;

	}
    void Update()
	{
		timer -= Time.deltaTime;

		if (isReloading)
		{
			if (timer > 0)
				return;
			else
			{
				isReloading = false;
				remainAmmo = maxAmmo;
			}
		}

		if (remainAmmo <= 0)
		{
			isReloading = true;
			timer = reloadTime;
			return;
		}

		if(Input.GetKey(spawnKey) && timer <= 0 && remainAmmo > 0)
		{
			remainAmmo--;
			SpawnProjectile();
		}
	}	

	void SpawnProjectile()
	{
		timer = spawnRate;

		BulletsPool.current.Take(bulletType, spawnPoint.position, spawnPoint.rotation);

		if (spawnParticles)
		{
			spawnParticles.Play();
		}

		if(spawnAudioSource)
		{
			spawnAudioSource.Play();
		}
	}
}
