using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour 
{
	private AstroidSpawn mAS;

	// Use this for initialization
	void Start () 
	{
		mAS = InGame.Instance.AstroidSpawn ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	void OnTriggerEnter(Collider col)
	{
		if ((col.gameObject != gameObject) && (!col.isTrigger))
		{
			mAS.SpawnAstCollisionEffects(transform.position);
			gameObject.SetActive(false);
			mAS.SpawnBulletCollisionEffects(transform.position);
		}
	}
}
