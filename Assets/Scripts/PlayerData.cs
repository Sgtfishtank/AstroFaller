using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour 
{
	private enum CashType
	{
		Bolts,
		Crystals
	}

	// snigleton
	private static PlayerData instance = null;
	public static PlayerData Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("PlayerData");
				if (thisObject ==  null)
				{
					thisObject = new GameObject("PlayerData");
					thisObject.AddComponent<PlayerData>();
					thisObject.GetComponent<PlayerData>().mBolts = 9999;
					thisObject.GetComponent<PlayerData>().mCrystals = 9999;
				}

				instance = thisObject.GetComponent<PlayerData>();
				DontDestroyOnLoad(instance.gameObject);
			}
			return instance;
		}
	}

	private int mBolts;
	private int mCrystals;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool withdrawCrystals (int amount)
	{
		if (amount < 0)
		{
			print("Error amount in withdrawCrystals " + amount);
			return false;
		}

		if (mCrystals >= amount)
		{
			mCrystals -= amount;
			return true;
		}

		return false;
	}

	public bool withdrawBolts(int amount)
	{
		if (amount < 0)
		{
			print("Error amount in withdrawBolts " + amount);
			return false;
		}

		if (mBolts >= amount)
		{
			mBolts -= amount;
			return true;
		}

		return false;
	}
	
	public bool depositBolts(int amount)
	{
		if (amount < 0)
		{
			print("Error amount in depositBolts " + amount);
			return false;
		}

		mBolts += amount;
		return true;
	}

	public bool depositCrystals(int amount)
	{
		if (amount < 0)
		{
			print("Error amount in depositCrystals " + amount);
			return false;
		}

		mCrystals += amount;
		return true;
	}
}
