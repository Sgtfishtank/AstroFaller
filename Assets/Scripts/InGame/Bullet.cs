using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	private SpawnerBase mAS;

	// Use this for initialization
	void Start () 
	{
		mAS = InGame.Instance.BaseSpawner ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if ((col.gameObject != gameObject) && (!col.isTrigger))
		{
			mAS.SpawnCollisionEffects(transform.position);
			gameObject.SetActive(false);
			//mAS.SpawnBulletCollisionEffects(transform.position);
		}
	}
}
