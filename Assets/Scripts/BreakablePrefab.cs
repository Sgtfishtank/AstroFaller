using UnityEngine;
using System.Collections;

public class BreakablePrefab : MonoBehaviour {

	public Vector2 mSpeed;
	public float mDealay;
	public float time = -1;
	bool first = true;
	public Rigidbody mRigidBody;
	public Rigidbody[] mPartRigidBodys;
	public Collider[] mPartColliders;
	public Collider mCollider;

	// Use this for initialization
	void Start ()
	{
		mRigidBody = GetComponent<Rigidbody>();
		mCollider = GetComponent<Collider>();
		mPartColliders = GetComponentsInChildren<Collider>();
		mPartRigidBodys = GetComponentsInChildren<Rigidbody>();
		time = Time.time + mDealay;
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
		//mBreak = false;
	}
	void Awake()
	{
		print("hej");
		time = Time.time + mDealay;
	}
	
	// Update is called once per frame
	void Update ()
	{
		print (Time.time);
		if(time > Time.time || time == -1)
		{
			print ("fisk");
			return;
		}
		else if(first)
		{
			//mSpeed.y += WorldGen.Instance.Player().GetComponent<Rigidbody>().velocity.y;
			gameObject.GetComponent<Rigidbody>().velocity=mSpeed;
			first = false;
		}

	
	}
}
