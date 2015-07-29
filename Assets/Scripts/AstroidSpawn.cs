using UnityEngine;
using System.Collections;

public class AstroidSpawn : MonoBehaviour {

	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public int mMaxAstroids = 5;
	public GameObject[] mAstroids;
	public float mCd = 5;
	public float mXspawn = 10;
	public float mRotationSpeed;

	private int index;
	private GameObject mPlayerObj;
	private Rigidbody mPlRigid;
	public float mLastSpawn = 0;

	void Start ()
	{
		mAstroids = new GameObject[mMaxAstroids];
		mPlayerObj = GameObject.Find ("Player");
		mPlRigid = mPlayerObj.GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.time > mLastSpawn+mCd)
		{
			mLastSpawn = Time.time +mCd;
			int x = UnityEngine.Random.Range(0,2)*2-1;
			int y = UnityEngine.Random.Range(-12,10);
			int astroid = UnityEngine.Random.Range(0,3);
			float angel = UnityEngine.Random.Range(0,360);
			mAstroids[index] = Instantiate(mAstroidTypes[astroid],
			                              new Vector3(mXspawn * x, mPlayerObj.transform.position.y 	 +y , 0),
			                              Quaternion.Euler(Vector3.one*angel)) as GameObject;
			mAstroids[index].GetComponent<Rigidbody>().velocity = new Vector3(
				UnityEngine.Random.Range(2,5)*-x, mPlRigid.velocity.y-y, 0);
			mAstroids[index].GetComponent<Rigidbody>().AddTorque(
				new Vector3(UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed),
			            	UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed),
			            	UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed)));
			if(mMaxAstroids-1 > index)
			{
				index++;
				if(mAstroids[index] != null)
				{
					Destroy(mAstroids[index]);
					mAstroids[index] = null;
				}
			}
			else if(index == mMaxAstroids-1)
			{
				index = 0;
				if(mAstroids[index] != null)
				{
					Destroy(mAstroids[index]);
					mAstroids[index] = null;
				}
			}
		}
	}
}
