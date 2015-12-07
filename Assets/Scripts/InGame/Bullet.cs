using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if ((col.gameObject != gameObject) && (!col.isTrigger))
		{
			InGame.Instance.SpawnBulletCollisionEffects(transform.position);
			gameObject.SetActive(false);
		}
	}
}
