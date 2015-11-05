using UnityEngine;
using System.Collections;

public class BreakableAstroids : MonoBehaviour 
{
	public Rigidbody mRigidBody;
	public Rigidbody[] mPartRigidBodys;
	public Collider[] mPartColliders;
	public Collider mCollider;
	public bool mBreak = false;
	public string mCollWith;

	void Awake()
	{
		mRigidBody = GetComponent<Rigidbody>();
		mCollider = GetComponent<Collider>();
		mPartColliders = GetComponentsInChildren<Collider>();
		mPartRigidBodys = GetComponentsInChildren<Rigidbody>();
		
		for(int i = 0; i < mPartRigidBodys.Length; i++)
		{
			mPartRigidBodys[i].mass = mRigidBody.mass / mPartColliders.Length;
		}
	}

	// Use this for initialization
	void Start ()
	{
	}

	void OnDisable()
	{
		for(int i = 1; i < mPartRigidBodys.Length; i++)
		{
			if (mPartRigidBodys[i] != null) 
			{
				mPartRigidBodys[i].velocity = Vector3.zero;
				mPartRigidBodys[i].transform.localPosition = Vector3.zero;
				mPartRigidBodys[i].transform.localRotation = Quaternion.identity;
				mPartRigidBodys[i].position = mPartRigidBodys[i].transform.position;
				mPartRigidBodys[i].rotation = mPartRigidBodys[i].transform.rotation;
				mPartRigidBodys[i].isKinematic = true;
				mPartColliders[i].enabled = false;
			}
		}
		mRigidBody.isKinematic = false;
		mCollider.enabled = true;
		mBreak = false;
	}

	// Update is called once per frame
	void Update ()
	{
	}
	
	void OnCollisionEnter(Collision col)
	{
		if((col.transform.tag == mCollWith) && (!mBreak))
		{
			if ((mCollWith == "Player") && (!InGame.Instance.Player().isBursting())) 
			{
				return;
			}

			float angle = Random.value * 360;
			for (int i = 1; i < mPartColliders.Length; i++) 
			{
				mPartRigidBodys[i].velocity = mRigidBody.velocity + (new Vector3(Mathf.Cos(Mathf.Deg2Rad * (angle + (i * 120))), Mathf.Sin(Mathf.Deg2Rad * (angle + (i * 120))), 0) * GlobalVariables.Instance.ASTROID_SPAWN_IMPACT_FACTOR);
				mPartRigidBodys[i].isKinematic = false;
				mPartColliders[i].enabled = true;
			}
			mRigidBody.isKinematic = true;
			mCollider.enabled = false;
			mBreak = true;
		}
	}
}
