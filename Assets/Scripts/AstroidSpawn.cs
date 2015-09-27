using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : MonoBehaviour {

	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public GameObject[] mAstroids;

	private Player mPlayer;
	private Rigidbody mPlRigid;

	private float mLastSpawn = 0;
	public int mSpawnedAsteroids;

	void Awake ()
	{
		mSpawnedAsteroids = 0;
		mAstroids = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_ASTROIDS];
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			int astroid = UnityEngine.Random.Range(0, mAstroidTypes.Length);
			mAstroids[i] = Instantiate(mAstroidTypes[astroid]) as GameObject;
			mAstroids[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mAstroids[i].SetActive(false);
		}
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

		if((Time.time > (mLastSpawn + mCd)) && (mSpawnedAsteroids < mMaxAstroids))
		{
			mLastSpawn = Time.time + mCd;
			int x = UnityEngine.Random.Range(0,2)*2-1;
			float y = UnityEngine.Random.Range(-25,8);
			//int astroid = UnityEngine.Random.Range(0,3);
			Quaternion angel = UnityEngine.Random.rotation;

			Vector3 pos = new Vector3(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * x, mPlayer.transform.position.y + y , 0);

			//Spawn astroid
			GameObject instace = PickFreeAsteroid();//Instantiate(mAstroidTypes[astroid], pos, angel) as GameObject;
			instace.transform.position = pos;
			instace.transform.rotation = angel;
			instace.SetActive(true);
			mSpawnedAsteroids++;

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

		}
	}

	GameObject PickFreeAsteroid()
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			if (!mAstroids[i].activeSelf)
			{
				return mAstroids[i];
			}
		}

		return null;
	}

	public void ShiftBack (float shift)
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i].transform.position -= new Vector3(0, shift, 0);
		}
	}

	public void RemoveAstroid(GameObject g)
	{
		g.SetActive(false);
		mSpawnedAsteroids--;
	}

	public void RemoveAllAstroids ()
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i].SetActive(false);
		}
		mSpawnedAsteroids = 0;
	}
}
