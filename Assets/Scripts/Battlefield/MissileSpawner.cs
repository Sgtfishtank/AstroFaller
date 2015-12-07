using UnityEngine;
using System.Collections;

public class MissileSpawner : SpawnerBase
{

	// Use this for initialization
	public GameObject mMissilePrefab;
	private GameObject[] mMissiles;
	
	private ParticleManager mMissileCollParticleManager;
	
	private Player mPlayer;
	private float mLastSpawn = 0;
	
	
	void Awake ()
	{
		// creat collision efets
		mMissileCollParticleManager = GetComponent<ParticleManager>();
	}
	
	void Start ()
	{
		int maxParticles = GlobalVariables.Instance.SPAWN_COLLISON_MAX_PARTICLES;
		Transform parent = InGame.Instance.transform.Find("ParticlesGoesHere").transform;
		mMissileCollParticleManager.Load(maxParticles, parent);
		mPlayer = WorldGen.Instance.Player();
	}
	
	public override void LoadObjects()
	{
		if (mMissiles != null) 
		{
			UnloadObjects();
		}

		
		
		if (mMissilePrefab == null) 
		{
			Debug.LogError("No Missile to spawn");
			return;
		}
		
		mMissiles = new GameObject[GlobalVariables.Instance.MAX_SPAWN_OBJECTS];
		for (int i = 0; i < mMissiles.Length; i++) 
		{
			mMissiles[i] = Instantiate(mMissilePrefab) as GameObject;
			mMissiles[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mMissiles[i].SetActive(false);
		}
		
	}
	
	public override void UnloadObjects()
	{
		if (mMissiles == null) 
		{
			return;
		}
		
		for (int i = 0; i < mMissiles.Length; i++)
		{
			if (mMissiles[i].activeSelf) 
			{
				mMissiles[i].SetActive(false);
			}
			Destroy(mMissiles[i]);
		}
		
		mMissiles = null;
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
		for (int i = 0; i < mMissiles.Length; i++) 
		{
			if (mMissiles[i].activeSelf && OutOfBound(mMissiles[i])) 
			{
				mMissiles[i].SetActive(false);
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
			GameObject instace = PickFreeMissile();//Instantiate(mAstroidTypes[astroid], pos, angel) as GameObject;
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
			
		}
	}
	
	GameObject PickFreeMissile()
	{
		for (int i = 0; i < mMissiles.Length; i++) 
		{
			if (!mMissiles[i].activeSelf)
			{
				return mMissiles[i];
			}
		}
		
		return null;
	}
	
	public override void ShiftBack (float shift)
	{
		for (int i = 0; i < mMissiles.Length; i++) 
		{
			mMissiles[i].transform.position -= new Vector3(0, shift, 0);
		}	
		mMissileCollParticleManager.ShiftBack(shift);

	}
	
	public override void Reset ()
	{
		for (int i = 0; i < mMissiles.Length; i++) 
		{
			mMissiles[i].SetActive(false);
		}
	}

	public override void SpawnCollisionEffects (Vector3 position)
	{
		mMissileCollParticleManager.Spawn(position);
	}
}
