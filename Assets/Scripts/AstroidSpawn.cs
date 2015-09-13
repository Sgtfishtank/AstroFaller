using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : MonoBehaviour {

	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public  List<GameObject> mAstroids;

	private Player mPlayer;
	private Rigidbody mPlRigid;

	private float mLastSpawn = 0;

	void Awake ()
	{
		mAstroids = new List<GameObject>();
	}

	void Start ()
	{
		mPlayer = WorldGen.Instance.Player();
	}
	
	// Update is called once per frame
	void Update ()
	{
		int mMaxAstroids = GlobalVariables.Instance.ASTROID_SPAWN_MAX_ASTROIDS;
		float mCd = GlobalVariables.Instance.ASTROID_SPAWN_SPAWNRATE;
		float mRotationSpeed = GlobalVariables.Instance.ASTROID_SPAWN_ROTATION_SPEED;

		mPlRigid = mPlayer.GetComponent<Rigidbody>();

		if((Time.time > (mLastSpawn + mCd)) && (mAstroids.Count < mMaxAstroids))
		{
			mLastSpawn = Time.time + mCd;
			int x = UnityEngine.Random.Range(0,2)*2-1;
			float y = UnityEngine.Random.Range(-25,8);
			int astroid = UnityEngine.Random.Range(0,3);
			Quaternion angel = UnityEngine.Random.rotation;

			//Spawn astroid
			GameObject instace = Instantiate(mAstroidTypes[astroid],
			                                 new Vector3(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * x, mPlayer.transform.position.y + y , 0),
			                                 angel) as GameObject;

			//add velocity
			Vector3 randVel = new Vector3(UnityEngine.Random.Range(2,5)*(-x), y, 0);

			Vector3 targetVel = mPlayer.transform.position - instace.transform.position;
			targetVel.y += mPlRigid.velocity.y;

			instace.GetComponent<Rigidbody>().velocity = Vector3.Lerp(targetVel, randVel, Random.value);

			//add torque
			instace.GetComponent<Rigidbody>().AddTorque(
				new Vector3(UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed),
			            UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed),
			            UnityEngine.Random.Range(-mRotationSpeed,mRotationSpeed)));
			mAstroids.Add(instace);
			
			instace.transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
		}
	}

	public void ShiftBack (float shift)
	{
		for (int i = 0; i < mAstroids.Count; i++) 
		{
			mAstroids[i].transform.position -= new Vector3(0, shift, 0);
		}
	}

	public void RemoveAstroid(GameObject g)
	{
		mAstroids.Remove(g);
		Destroy(g);
	}

	public void RemoveAllAstroids ()
	{
		for (int i = 0; i < mAstroids.Count; i++) 
		{
			Destroy(mAstroids[i]);
		}
		mAstroids.Clear ();
	}
}
