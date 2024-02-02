using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveGameObject : MonoBehaviour {

	public float lifeTime;

	void OnEnable()
	{
		lifeTime = 2f;
	}
	void Update()
	{
		lifeTime -= Time.deltaTime;
		if (lifeTime < 0)
			gameObject.SetActive(false);
	}
}
