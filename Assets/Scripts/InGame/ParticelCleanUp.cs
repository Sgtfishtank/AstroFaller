using UnityEngine;
using System.Collections;

public class ParticelCleanUp : MonoBehaviour 
{
	public bool mDeactiveInstead;

	public float mEndTime;
	public ParticleSystem[] mPSs;
	// Use this for initialization
	void Start ()
	{
		mPSs = gameObject.GetComponentsInChildren<ParticleSystem>();
	}

	public void Activate(float timeActivated)
	{
		mEndTime = Time.time + timeActivated;
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

		if (mPSs.Length == 0)
		{
			if (mEndTime > Time.time)
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
