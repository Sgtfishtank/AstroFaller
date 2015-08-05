using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : MonoBehaviour {

	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public int mMaxAstroids = 5;
	public static List<GameObject> mAstroids;
	public float mCd = 5;
	public float mXspawn = 10;
	public float mRotationSpeed;

	private int index;
	private GameObject mPlayerObj;
	private Rigidbody mPlRigid;
	public float mLastSpawn = 0;

	void Start ()
	{
		mAstroids = new List<GameObject>();
		mPlayerObj = GameObject.Find ("Player");
		mPlRigid = mPlayerObj.GetComponentInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Time.time > mLastSpawn+mCd && mAstroids.Count < mMaxAstroids)
		{
			mLastSpawn = Time.time +mCd;
			int x = UnityEngine.Random.Range(0,2)*2-1;
			int y = UnityEngine.Random.Range(-12,5);
			int astroid = UnityEngine.Random.Range(0,3);
			Quaternion angel = UnityEngine.Random.rotation;
			//Spawn astroid

			GameObject instace = Instantiate(mAstroidTypes[astroid],
			                                 new Vector3(mXspawn * x, mPlayerObj.transform.position.y +y , 0),
			                                 angel) as GameObject;
			//add velocity
			instace.GetComponent<Rigidbody>().velocity = new Vector3(
				UnityEngine.Random.Range(2,5)*-x, mPlRigid.velocity.y-y, 0);
			//add torque
			instace.GetComponent<Rigidbody>().AddTorque(
				new Vector3(UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed),
			            UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed),
			            UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed)));
			mAstroids.Add(instace);
		}
	}
	public static void RemoveAstroid(GameObject g)
	{
		mAstroids.Remove(g);
	}
}
