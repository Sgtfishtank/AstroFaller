using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AstroidSpawn : MonoBehaviour 
{
	// Use this for initialization
	public GameObject[] mAstroidTypes;
	public GameObject[] mMissilePrefabs;
	private GameObject[] mAstroids;

	private ParticleManager mAstCollParticle1Manager;
	private ParticleManager mAstCollParticle2Manager;
	private ParticleManager mBulletCollParticleManager;
	private ParticleManager mMissileCollParticleManager;

	public GameObject mPlayerAsteroidPrefab;
	private GameObject mPlayerAsteroid;
	private float mPlayerAsteroidT;

	private Player mPlayer;
	private float mLastSpawn = 0;
	private int mSpawnedAsteroids;
	private bool mSpawning;

	void Awake ()
	{
		// creat collision efets
		mAstCollParticle1Manager = GetComponents<ParticleManager>()[0];
		mAstCollParticle2Manager = GetComponents<ParticleManager>()[1];
		mBulletCollParticleManager = GetComponents<ParticleManager>()[2];
		mMissileCollParticleManager = GetComponents<ParticleManager>()[3];
		mAstCollParticle1Manager.Load(GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES);
		mAstCollParticle2Manager.Load(GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES);
		mBulletCollParticleManager.Load(GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES);
		mMissileCollParticleManager.Load(GlobalVariables.Instance.ASTROID_SPAWN_MAX_PARTICLES);

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
		if (mAstroids != null) 
		{
			UnloadAsteroids();
		}

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
		
		mAstroids = new GameObject[GlobalVariables.Instance.ASTROID_SPAWN_MAX_ASTROIDS];
		for (int i = 0; i < mAstroids.Length; i++) 
		{
			mAstroids[i] = Instantiate(arrPrefabs[i % arrPrefabs.Length]) as GameObject;
			mAstroids[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mAstroids[i].SetActive(false);
		}

		mSpawnedAsteroids = 0;
	}
	
	public void UnloadAsteroids()
	{
		if (mAstroids == null) 
		{
			return;
		}

		for (int i = 0; i < mAstroids.Length; i++)
		{
			if (mAstroids[i].activeSelf) 
			{
				RemoveAstroid(mAstroids[i]);
			}
			Destroy(mAstroids[i]);
		}

		mSpawnedAsteroids = 0;
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
			if (mAstroids[i].activeSelf && OutOfBound(mAstroids[i])) 
			{
				RemoveAstroid(mAstroids[i]);
			}
		}

		if (mSpawning) 
		{
			SpawnAsteroid();
		}
	}

	void SpawnAsteroid ()
	{
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

	public bool OutOfBound(GameObject asteroid)
	{
		Vector3 pos = asteroid.transform.position;
		float xMax = GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f;
		float xMin = -GlobalVariables.Instance.ASTROID_SPAWN_XOFFSET * 1.5f;
		float yMax = mPlayer.transform.position.y + 25;
		float yMin = mPlayer.transform.position.y - 50;
		
		return ((pos.x > xMax) || (pos.x < xMin) || (pos.y > yMax) || (pos.y < yMin));
	}

	public void SpawnAstCollisionEffects(Vector3 position)
	{
		mAstCollParticle1Manager.Spawn(position);
		mAstCollParticle2Manager.Spawn(position);
	}

	public void SpawnBulletCollisionEffects (Vector3 position)
	{
		mBulletCollParticleManager.Spawn(position);
	}
	
	public void SpawnMissileCollisionEffects (Vector3 position)
	{
		mMissileCollParticleManager.Spawn(position);
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

		mAstCollParticle1Manager.ShiftBack(shift);
		mAstCollParticle2Manager.ShiftBack(shift);
		mBulletCollParticleManager.ShiftBack(shift);
		mMissileCollParticleManager.ShiftBack(shift);
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

	public void StopSpawning ()
	{
		mSpawning = false;
	}
	
	public void StartSpawning ()
	{
		mSpawning = true;
	}
}
