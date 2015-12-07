using UnityEngine;
using System.Collections;

public class LightningSpawner : SpawnerBase
{
	
	// Use this for initialization
	public GameObject mLightningPrefab;
	private GameObject[] mLightnings;
	
	private Player mPlayer;
	private float mLastSpawn = 0;
	
	
	void Awake ()
	{
	}
	
	void Start ()
	{
		mPlayer = WorldGen.Instance.Player();
	}
	
	public override void LoadObjects()
	{
		if (mLightnings != null) 
		{
			UnloadObjects();
		}
		
		
		
		if (mLightningPrefab == null) 
		{
			Debug.LogError("No Lightning to spawn");
			return;
		}
		
		mLightnings = new GameObject[GlobalVariables.Instance.MAX_SPAWN_OBJECTS];
		for (int i = 0; i < mLightnings.Length; i++) 
		{
			mLightnings[i] = Instantiate(mLightningPrefab) as GameObject;
			mLightnings[i].transform.parent = InGame.Instance.transform.Find("AstroidsGoesHere");
			mLightnings[i].SetActive(false);
		}
		
	}
	
	public override void UnloadObjects()
	{
		if (mLightnings == null) 
		{
			return;
		}
		
		for (int i = 0; i < mLightnings.Length; i++)
		{
			if (mLightnings[i].activeSelf) 
			{
				mLightnings[i].SetActive(false);
			}
			Destroy(mLightnings[i]);
		}
		
		mLightnings = null;
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
		for (int i = 0; i < mLightnings.Length; i++) 
		{
			if (mLightnings[i].activeSelf && InGame.Instance.OutOfSegmentBounds(mLightnings[i])) 
			{
				mLightnings[i].SetActive(false);
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
		for (int i = 0; i < mLightnings.Length; i++) 
		{
			if (!mLightnings[i].activeSelf)
			{
				return mLightnings[i];
			}
		}
		
		return null;
	}
	
	public override void ShiftBack (float shift)
	{
		for (int i = 0; i < mLightnings.Length; i++) 
		{
			mLightnings[i].transform.position -= new Vector3(0, shift, 0);
		}	
		
	}

	public override void Reset ()
	{
		for (int i = 0; i < mLightnings.Length; i++) 
		{
			mLightnings[i].SetActive(false);
		}
	}

}