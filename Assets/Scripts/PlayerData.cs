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
				instance = thisObject.GetComponent<PlayerData>();
			}
			return instance;
		}
	}

	public int mBolts;
	public int mCrystals;
	private int mTotalBolts;
	private int mTotalCrystals;
	private int mTotalDistance;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public int bolts()
	{
		return mBolts;
	}

	public int crystals()
	{
		return mCrystals;
	}
	
	public int totalBolts()
	{
		return mTotalBolts;
	}
	
	public int totalCrystals()
	{
		return mTotalCrystals;
	}
	
	public int totalDistance()
	{
		return mTotalDistance;
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
		mTotalBolts += amount;
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
		mTotalCrystals += amount;
		return true;
	}

	public bool depositDistance (int amount)
	{
		if (amount < 0)
		{
			print("Error amount in depositDistance " + amount);
			return false;
		}

		mTotalDistance += amount;
		return true;
	}
}
