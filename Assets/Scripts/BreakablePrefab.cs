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
	public string mCollWith;
	public bool mBreak;

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
		print("hej "+gameObject.name);
		time = Time.time + mDealay;
		first=true;
	}
	void OnEnable()
	{
		time = Time.time + mDealay;
	}
	void OnDisable()
	{
		time= -1;
		first =true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(time > Time.time || time == -1)
		{
			return;
		}
		else if(first)
		{
			//mSpeed.y += WorldGen.Instance.Player().GetComponent<Rigidbody>().velocity.y;
			gameObject.GetComponent<Rigidbody>().velocity=mSpeed;
			first = false;
		}

	
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
