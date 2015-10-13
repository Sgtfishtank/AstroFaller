using UnityEngine;
using System.Collections;

public class BreakableAstroids : MonoBehaviour 
{
	private Rigidbody mRigidBody;
	private Rigidbody[] mPartRigidBodys;
	private Collider[] mPartColliders;
	private Collider mCollider;
	private bool mBreak = false;
	public string mCollWith;

	void Awake()
	{
		mRigidBody = GetComponent<Rigidbody>();
		mCollider = GetComponent<Collider>();
		mPartColliders = GetComponentsInChildren<Collider>();
		mPartRigidBodys = GetComponentsInChildren<Rigidbody>();
	}

	// Use this for initialization
	void Start ()
	{
		/*for(int i = 0; i < mPartRigidBodys.Length; i++)
		{
			mRigid[i].constraints = RigidbodyConstraints.FreezeAll;
		}*/
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
				//Destroy(mPartRigidBodys[i]);
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
		/*if(!mBreak)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(0,0,0), 5 * Time.deltaTime);
		}*/
	}
	
	void OnCollisionEnter(Collision col)
	{
		if((col.transform.tag == mCollWith) && (!mBreak))
		{
			float angle = Random.value * 360;
			for (int i = 1; i < mPartColliders.Length; i++) 
			{
				//mPartRigidBodys[i] = mPartColliders[i].gameObject.AddComponent<Rigidbody>();
				mPartRigidBodys[i].velocity = mRigidBody.velocity + (new Vector3(Mathf.Cos(Mathf.Deg2Rad * (angle + (i * 120))), Mathf.Sin(Mathf.Deg2Rad * (angle + (i * 120))), 0) * GlobalVariables.Instance.ASTROID_SPAWN_IMPACT_FACTOR);
				mPartRigidBodys[i].mass = mRigidBody.mass / mPartColliders.Length;
				mPartRigidBodys[i].constraints = mRigidBody.constraints;
				mPartRigidBodys[i].useGravity = false;
				mPartRigidBodys[i].isKinematic = false;
				mPartColliders[i].enabled = true;
			}
			mRigidBody.isKinematic = true;
			mCollider.enabled = false;
			mBreak = true;
			/*for(int i =0;i < mRigid.Length; i++)
			{
				mRigid[i].constraints = RigidbodyConstraints.None;
				mRigid[i].constraints = RigidbodyConstraints.FreezePositionZ;
			}*/
		}
	}
}
