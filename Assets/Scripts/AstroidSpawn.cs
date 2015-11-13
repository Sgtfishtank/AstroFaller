using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : MonoBehaviour 
{
	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public GameObject[] mMissilePrefabs;
	private GameObject[] mAstroids;

	public GameObject mCollisionEffect1Prefab;
	public GameObject mCollisionEffect2Prefab;
	private GameObject[] mCollisionEffects1;
	private GameObject[] mCollisionEffects2;
	
	public GameObject mPlayerAsteroidPrefab;
	private GameObject mPlayerAsteroid;
	private float mPlayerAsteroidT;

	private Player mPlayer;
	private float mLastSpawn = 0;
	private int mSpawnedAsteroids;

	void Awake ()
	{
		mSpawnedAsteroids = 0;
		mAstroids = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_ASTROIDS];

		// creat collision efets
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

		// creat player dasdoiud
		mPlayerAsteroid = Instantiate(mPlayerAsteroidPrefab) as GameObject;
		mPlayerAsteroid.SetActive (false);
	}

	void Start ()
	{
		mPlayer = WorldGen.Instance.Player();
	}

	public void LoadAsteroids(int levelIndex)
	{
		GameObject[] arrPrefabs = null;

		switch (levelIndex) 
		{
		case 1:
			arrPrefabs = mAstroidTypes;
			break;
		case 2:
			arrPrefabs = mMissilePrefabs;
			break;
		}

		if (arrPrefabs.Length == 0) 
		{
			Debug.LogError("No asteroids to spawn");
			return;
		}

		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i] = Instantiate(arrPrefabs[i % arrPrefabs.Length]) as GameObject;
			mAstroids[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mAstroids[i].SetActive(false);
		}
	}
	
	public void UnloadAsteroids()
	{
		for (int i = 0; i < mAstroids.Length; i++)
		{
			if (mAstroids[i].activeSelf) 
			{
				RemoveAstroid(mAstroids[i]);
			}
			Destroy(mAstroids[i]);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			if (mAstroids[i].activeSelf && OutOfBound(mAstroids[i])) 
			{
				RemoveAstroid(mAstroids[i]);
			}
		}

		int mMaxAstroids = GlobalVariables.Instance.ASTROID_SPAWN_MAX_ASTROIDS;
		float mCd = GlobalVariables.Instance.ASTROID_SPAWN_SPAWNRATE;

		Rigidbody playerRb = mPlayer.GetComponent<Rigidbody>();

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
			mSpawnedAsteroids++;

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
				mPlayerAsteroid.transform.position = new Vector3(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * x, mPlayer.transform.position.y - playerBreakableOffset, 0);
				Vector3 arandVel = new Vector3(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * -x, playerRb.velocity.y, 0);
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
		if ((absx > GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET) || (mPlayerAsteroidT < Time.time))
		{
			return true;
		}
		return false;
	}
	
	bool OutOfBoundOLD(GameObject asteroid)
	{
		return (!(transform.position.x < (GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f) && 
		          transform.position.x > -(GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f) &&
		          transform.position.y < (mPlayer.transform.position.y + 25) && 
		          transform.position.y > (mPlayer.transform.position.y - 50)));
	}

	bool OutOfBound(GameObject asteroid)
	{
		Vector3 pos = asteroid.transform.position;
		float xMax = GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f;
		float xMin = -GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f;
		float yMax = mPlayer.transform.position.y + 25;
		float yMin = mPlayer.transform.position.y - 50;
		
		return ((pos.x > xMax) || (pos.x < xMin) || (pos.y > yMax) || (pos.y < yMin));
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
		mPlayerAsteroid.transform.position -= new Vector3(0, shift, 0);
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
