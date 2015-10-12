using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : MonoBehaviour {

	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public GameObject[] mAstroids;

	public GameObject mCollisionEffect1Prefab;
	public GameObject mCollisionEffect2Prefab;
	public GameObject[] mCollisionEffects1;
	public GameObject[] mCollisionEffects2;

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
			mAstroids[i] = Instantiate(mAstroidTypes[i % mAstroidTypes.Length]) as GameObject;
			mAstroids[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mAstroids[i].SetActive(false);
		}
		
		mCollisionEffects1 = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES];
		mCollisionEffects2 = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES];
		for (int i = 0; i < mCollisionEffects1.Length; i++)
		{
			mCollisionEffects1[i] = (GameObject)GameObject.Instantiate(mCollisionEffect1Prefab);
			mCollisionEffects2[i] = (GameObject)GameObject.Instantiate(mCollisionEffect2Prefab);
			mCollisionEffects1[i].transform.parent = InGame.Instance.transform.Find("ParticlesGoesHere");
			mCollisionEffects2[i].transform.parent = InGame.Instance.transform.Find("ParticlesGoesHere");
			mCollisionEffects1[i].gameObject.SetActive(false);
			mCollisionEffects2[i].gameObject.SetActive(false);
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

			Rigidbody rb = instace.GetComponent<Rigidbody>();
			rb.velocity = Vector3.Lerp(targetVel, randVel, Random.value);
			
			//add torque
			rb.AddTorque(
				new Vector3(UnityEngine.Random.Range(-mRotationSpeed, mRotationSpeed),
			            UnityEngine.Random.Range(-mRotationSpeed, mRotationSpeed),
			            UnityEngine.Random.Range(-mRotationSpeed, mRotationSpeed)));
		}
	}

	public void SpawnCollisionEffects(Vector3 position)
	{
		int index  = PickCollisionEffect();
		if (index != -1) 
		{
			mCollisionEffects1[index].SetActive(true);
			mCollisionEffects1[index].transform.position = position;
			mCollisionEffects1[index].transform.rotation = Quaternion.identity;

			mCollisionEffects2[index].SetActive(true);
			mCollisionEffects2[index].transform.position = position;
			mCollisionEffects2[index].transform.rotation = Quaternion.identity;
		}
	}

	int PickCollisionEffect()
	{
		for (int i = 0; i < mCollisionEffects1.Length; i++) 
		{
			if ((!mCollisionEffects1[i].activeSelf) && (!mCollisionEffects2[i].activeSelf))
			{
				return i;
			}
		}
		
		return -1;
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
