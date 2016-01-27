using UnityEngine;
using System.Collections;
using System.Linq;

public class Segment : MonoBehaviour 
{
	public int mStaticWeight;
	public int mLength;
	public Vector3[] mOriginalPosition;
	public Vector3[] mOriginalScale;
	public Quaternion[] mOriginalRotation;
	public Transform[] mTrans;
	public Rigidbody[] mRigidbodys;

	void Awake()
	{
		mTrans = gameObject.GetComponentsInChildren<Transform>(true).Where(x =>x.tag == "Enemy").ToArray();
		mRigidbodys = gameObject.GetComponentsInChildren<Rigidbody>(true).Where(x =>x.tag == "Enemy").ToArray();
		if(mOriginalPosition.Length == 0)
		{
			mOriginalPosition = new Vector3[mTrans.Length];
			mOriginalRotation = new Quaternion[mTrans.Length];
			mOriginalScale = new Vector3[mTrans.Length];
			for (int i = 0; i < mTrans.Length; i++)
			{
				mOriginalPosition[i] = mTrans[i].localPosition;
				mOriginalRotation[i] = mTrans[i].localRotation;
				mOriginalScale[i] = mTrans[i].localScale;
			}
		}
	}

	// Use this for initialization
	void Start () 
	{
	}

	void OnDisable()
	{
		for (int i = 0; i < mTrans.Length; i++)
		{
			mTrans[i].localPosition = mOriginalPosition[i];
			mTrans[i].localRotation = mOriginalRotation[i];
			mTrans[i].localScale = mOriginalScale[i];
		}

		for (int i = 0; i < mRigidbodys.Length; i++) 
		{
            mRigidbodys[i].velocity = Vector3.zero;
            mRigidbodys[i].angularVelocity = Vector3.zero;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
