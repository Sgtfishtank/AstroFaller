using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : SpawnerBase 
{
	// Use this for initialization
	public GameObject[] mAstroidTypes;
	private GameObject[] mAstroids;

	public GameObject mPlayerAsteroidPrefab;
	private GameObject mPlayerAsteroid;
	private float mPlayerAsteroidT;

	private Player mPlayer;
	private float mLastSpawn = 0;

	void Awake ()
	{
		// creat player dasdoiud
		mPlayerAsteroid = Instantiate(mPlayerAsteroidPrefab) as GameObject;
		mPlayerAsteroid.SetActive (false);
	}

	void Start ()
	{
		mPlayer = WorldGen.Instance.Player();
	}

	public override void LoadObjects()
	{
		if (mAstroids != null) 
		{
			UnloadObjects();
		}

		GameObject[] arrPrefabs = null;
		arrPrefabs = mAstroidTypes;


		if (arrPrefabs.Length == 0) 
		{
			Debug.LogError("No asteroids to spawn");
			return;
		}
		
		mAstroids = new GameObject[GlobalVariables.Instance.MAX_SPAWN_OBJECTS];
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i] = Instantiate(arrPrefabs[i % arrPrefabs.Length]) as GameObject;
			mAstroids[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mAstroids[i].SetActive(false);
		}

	}
	
	public override void UnloadObjects()
	{
		if (mAstroids == null) 
		{
			return;
		}

		for (int i = 0; i < mAstroids.Length; i++)
		{
			if (mAstroids[i].activeSelf) 
			{
				mAstroids[i].SetActive(false);
			}
			Destroy(mAstroids[i]);
		}

		mAstroids = null;
	}
	
	void OnEnable()
	{
		StopSpawning();
	}

	void OnDisable()
	{
		StopSpawning();
	}

	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			if (mAstroids[i].activeSelf && InGame.Instance.OutOfSegmentBounds(mAstroids[i])) 
			{
				mAstroids[i].SetActive(false);
			}
		}

		if (SpawnObjs) 
		{
			SpawnObject();
		}
	}

	public override void SpawnObject ()
	{
		float mCd = GlobalVariables.Instance.ASTROID_SPAWN_SPAWNRATE;
		
		Rigidbody playerRb = mPlayer.GetComponent<Rigidbody>();
		
		if((Time.time > (mLastSpawn + mCd)))
		{
			mLastSpawn = Time.time + mCd;
			int x = UnityEngine.Random.Range(0,2)*2-1;
			float y = UnityEngine.Random.Range(-25,8);
			//int astroid = UnityEngine.Random.Range(0,3);
			Quaternion angel = UnityEngine.Random.rotation;
			
			Vector3 pos = new Vector3(GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * x, mPlayer.transform.position.y + y , 0);
			
			//Spawn astroid
			GameObject instace = PickFreeAsteroid();//Instantiate(mAstroidTypes[astroid], pos, angel) as GameObject;
			if(instace == null)
			{
				return;
			}

			instace.transform.position = pos;
			instace.transform.rotation = angel;
			
			//add velocity
			Vector3 randVel = new Vector3(UnityEngine.Random.Range(2,5)*(-x), y, 0);
			
			Vector3 targetVel = mPlayer.transform.position - instace.transform.position;
			targetVel.y += playerRb.velocity.y;
			
			Rigidbody rb = instace.GetComponent<Rigidbody>();
			rb.velocity = Vector3.Lerp(targetVel, randVel, Random.value);
			
			instace.SetActive(true);
			
			float playerBreakableChance = 0.9f;
			
			float playerBreakableOffset = 12;
			float playerBreakableTime = 10;
			if ((!mPlayerAsteroid.activeSelf) && (Random.value < playerBreakableChance) && false) 
			{
				mPlayerAsteroid.SetActive(true);
				Rigidbody rba = mPlayerAsteroid.GetComponent<Rigidbody>();
				mPlayerAsteroid.transform.position = new Vector3(GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * x, mPlayer.transform.position.y - playerBreakableOffset, 0);
				Vector3 arandVel = new Vector3(GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X * -x, playerRb.velocity.y, 0);
				rba.velocity = arandVel;
				
				mPlayerAsteroidT = Time.time + playerBreakableTime;
			}
		}
		
		if ((mPlayerAsteroid.activeSelf) && (OutOfBoundsPlayer(mPlayerAsteroid)))
		{
			mPlayerAsteroid.SetActive(false);
		}
	}

	bool OutOfBoundsPlayer(GameObject playerAsteroid)
	{
		float absx = Mathf.Abs (mPlayerAsteroid.transform.position.x);
		if ((absx > GlobalVariables.Instance.SPAWNOBJ_LELVEL_BOUNDS_X) || (mPlayerAsteroidT < Time.time))
		{
			return true;
		}
		return false;
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

	public override void ShiftBack (float shift)
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i].transform.position -= new Vector3(0, shift, 0);
		}
		mPlayerAsteroid.transform.position -= new Vector3(0, shift, 0);
	}

	public override void Reset ()
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i].SetActive(false);
		}
	}
}
