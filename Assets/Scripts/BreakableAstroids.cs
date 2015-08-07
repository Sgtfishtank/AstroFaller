using UnityEngine;
using System.Collections;

public class BreakableAstroids : MonoBehaviour {

	public Rigidbody[] mRigid;
	private bool mBreak = false;
	public string mCollWith;
	// Use this for initialization
	void Start ()
	{
		mRigid = gameObject.GetComponentsInChildren<Rigidbody>();
		for(int i =0;i < mRigid.Length; i++)
		{
			mRigid[i].constraints = RigidbodyConstraints.FreezeAll;
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(!mBreak)
		{
			transform.position = Vector3.MoveTowards(transform.position, new Vector3(0,0,0), 5 * Time.deltaTime);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		print (col.transform.tag);
		if(col.transform.tag == mCollWith)
		{
			print("hhh");
			mBreak = true;
			for(int i =0;i < mRigid.Length; i++)
			{
				mRigid[i].constraints = RigidbodyConstraints.None;
				mRigid[i].constraints = RigidbodyConstraints.FreezePositionZ;
			}
		}
	}
}
