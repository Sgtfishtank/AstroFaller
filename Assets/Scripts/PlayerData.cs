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

	public int mAirPerkUnlockedLevel;
	public int mLifePerkUnlockedLevel;
	public int mBurstPerkUnlockedLevel;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	// air
	public float MaxAirTime()
	{
		switch (mAirPerkUnlockedLevel) 
		{
		case 0:
			return GlobalVariables.Instance.PLAYER_MAX_AIR;
		case 1:
			return GlobalVariables.Instance.PLAYER_MAX_AIR + GlobalVariables.Instance.AIR_PERK_MAIN_LEVELS[1];
		case 2: case 3:
			return GlobalVariables.Instance.PLAYER_MAX_AIR + GlobalVariables.Instance.AIR_PERK_MAIN_LEVELS[1] + GlobalVariables.Instance.AIR_PERK_LEFT_LEVELS[1];
		default:
			print("error air perk max air " + mAirPerkUnlockedLevel);
			return GlobalVariables.Instance.PLAYER_MAX_AIR;
		}
	}

	public bool UnlimitedAirOneLife()
	{
		return (mAirPerkUnlockedLevel == 3);
	}

	// burst
	public float BurstDelay()
	{
		float distance = GlobalVariables.Instance.PLAYER_DASH_SPEED / GlobalVariables.Instance.PLAYER_DASH_SPEED_DELAY;

		switch (mBurstPerkUnlockedLevel) 
		{
		case 0:
			distance += 0;
			break;
		case 1:
			distance += GlobalVariables.Instance.BURST_PERK_MAIN_LEVELS[1];
			break;
		case 2: case 3:
			distance += GlobalVariables.Instance.BURST_PERK_MAIN_LEVELS[1] + GlobalVariables.Instance.BURST_PERK_LEFT_LEVELS[1];
			break;
		default:
			distance += 0;
			print("error burst perk max BURST " + mBurstPerkUnlockedLevel);
			break;
		}

		float delay = distance / GlobalVariables.Instance.PLAYER_DASH_SPEED;
		return delay;
	}

	public float BurstCooldown()
	{
		switch (mBurstPerkUnlockedLevel) 
		{
		case 0: case 1: case 2: 
			return GlobalVariables.Instance.PLAYER_DASH_CD;
		case 3:
			return GlobalVariables.Instance.PLAYER_DASH_CD - GlobalVariables.Instance.BURST_PERK_RIGHT_LEVELS[1];
		default:
			print("error burst perk BurstCooldown " + mBurstPerkUnlockedLevel);
			return GlobalVariables.Instance.PLAYER_DASH_CD;
		}
	}

	// life
	public int MaxLife ()
	{
		switch (mLifePerkUnlockedLevel) 
		{
		case 0:
			return GlobalVariables.Instance.PLAYER_MAX_LIFE;
		case 1:
			return GlobalVariables.Instance.PLAYER_MAX_LIFE + GlobalVariables.Instance.LIFE_PERK_MAIN_LEVELS[1];
		case 2: case 3:
			return GlobalVariables.Instance.PLAYER_MAX_LIFE + GlobalVariables.Instance.LIFE_PERK_MAIN_LEVELS[1] + GlobalVariables.Instance.LIFE_PERK_LEFT_LEVELS[1];
		default:
			print("error life perk MaxLife " + mLifePerkUnlockedLevel);
			return GlobalVariables.Instance.PLAYER_MAX_LIFE;
		}
	}

	public bool RegenerateLifeAfterHit()
	{
		return (mLifePerkUnlockedLevel == 3);
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
