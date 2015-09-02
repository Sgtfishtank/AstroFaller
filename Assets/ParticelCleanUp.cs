using UnityEngine;
using System.Collections;

public class ParticelCleanUp : MonoBehaviour {

	private ParticleSystem mPS;
	// Use this for initialization
	void Start ()
	{
		mPS = gameObject.GetComponentInChildren<ParticleSystem>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(mPS.isStopped)
		{
			Destroy(gameObject);
		}
	}
}
