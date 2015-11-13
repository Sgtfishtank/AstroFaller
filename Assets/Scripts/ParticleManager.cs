using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour 
{
	public GameObject mPickupTextPrefab;
	public bool mForceSpawn;

	private GameObject[] mParticles;
	private float[] mParticleSpawnTimes;
	private int mActiveParticles;
	
	void Awake()
	{
	}

	public void Load(int size)
	{
		Load(size, GameObject.Find("Game/ParticlesGoesHere").transform);
	}

	public void Load(int size, Transform parent)
	{
		mParticles = new GameObject[size];
		if (mForceSpawn) 
		{
			mParticleSpawnTimes = new float[size];
		}
		for (int i = 0; i < mParticles.Length; i++) 
		{
			mParticles[i] = Instantiate(mPickupTextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			mParticles[i].SetActive(false);
			mParticles[i].transform.rotation = Quaternion.identity;
			mParticles[i].transform.parent = parent;
		}
	}

	public void Unload()
	{
		for (int i = 0; i < mParticles.Length; i++) 
		{
			mParticles[i].SetActive(false);
			Destroy(mParticles[i]);
		}

		mActiveParticles = 0;
		mParticles = null;
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public void reset()
	{
		for (int i = 0; i < mParticles.Length; i++) 
		{
			mParticles[i].SetActive(false);
		}

		mActiveParticles = 0;
	}

	public GameObject[] getParticles()
	{
		return mParticles;
	}

	public GameObject Spawn(Vector3 position)
	{
		int index = PickParticle ();
		if (index != -1) 
		{
			mParticles[index].transform.position = position;
			mParticles[index].SetActive(true);
			if (mParticleSpawnTimes != null) 
			{
				mParticleSpawnTimes[index] = Time.time;
			}
			mActiveParticles++;
			return mParticles[index];
		}
		else if (mParticleSpawnTimes != null)
		{
			index = PickOldestParticle();
			mParticles[index].SetActive(false);
			mParticles[index].transform.position = position;
			mParticles[index].SetActive(true);
			mParticleSpawnTimes[index] = Time.time;
			mActiveParticles++;
			return mParticles[index];
		}

		return null;
	}

	public void DespawnParticle (int i)
	{
		mParticles [i].SetActive (false);
		mActiveParticles--;
	}
	
	int PickParticle()
	{
		for (int i = 0; i < mParticles.Length; i++) 
		{
			if (!mParticles[i].activeSelf) 
			{
				return i;
			}
		}
		
		return -1;
	}
	
	int PickOldestParticle()
	{
		float min = mParticleSpawnTimes[0];
		int minIndex = 0;
		for (int i = 1; i < mParticleSpawnTimes.Length; i++) 
		{
			if (mParticleSpawnTimes[i] < min)
			{
				min = mParticleSpawnTimes[i];
				minIndex = i;
			}
		}
		
		return minIndex;
	}

	public void ShiftBack (float shift)
	{
		for (int i = 0; i < mParticles.Length; i++) 
		{
			mParticles[i].transform.position -= new Vector3(0, shift, 0);
		}
	}
}
