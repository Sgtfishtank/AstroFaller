using UnityEngine;
using System.Collections;

public class ParticelCleanUp : MonoBehaviour 
{
	public bool mDeactiveInstead;

	private ParticleSystem[] mPSs;
	// Use this for initialization
	void Start ()
	{
		mPSs = gameObject.GetComponentsInChildren<ParticleSystem>();	
	}
	
	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < mPSs.Length; i++) 
		{
			if (!mPSs[i].isStopped)
			{
				return;
			}
		}

		if (mDeactiveInstead)
		{
			gameObject.SetActive(false);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
