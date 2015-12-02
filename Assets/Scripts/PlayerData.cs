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
                GameObject.DontDestroyOnLoad(thisObject);
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
    public InGame.Level LevelToLoad;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	// air
	public bool UnlimitedAirOneLife()
	{
		return (mAirPerkUnlockedLevel >= 3);
	}
	
	public float MaxAirTime()
	{
		if (mAirPerkUnlockedLevel >= 1) 
		{
			return GlobalVariables.Instance.PLAYER_MAX_AIR * 2; // double air time - WHEEEEEEEEEEEEEEEEEESH!
		}

		return GlobalVariables.Instance.PLAYER_MAX_AIR;
	}
	
	public bool NoExplodeWhenNoAir()
	{
		return (mAirPerkUnlockedLevel >= 2);
	}

	// burst
	public int BurstMultiplier()
	{
		if (mBurstPerkUnlockedLevel >= 1) 
		{
			return 4;
		}

		return 2;
	}

	public int MaxBursts()
	{
		if (mBurstPerkUnlockedLevel >= 3) 
		{
			return 2;
		}
		
		return 1;
	}

	public float BurstCooldown()
	{
		if (mBurstPerkUnlockedLevel >= 2) 
		{
			return GlobalVariables.Instance.PLAYER_DASH_CD / 2;
		}
		
		return GlobalVariables.Instance.PLAYER_DASH_CD;
	}

	// life
	public float TimeInvurnerableAfterHit()
	{
		if (mLifePerkUnlockedLevel >= 1) 
		{
			return 1;
		}

		return -1;
	}

	public int MaxLife ()
	{
		if (mLifePerkUnlockedLevel >= 2) 
		{
			return GlobalVariables.Instance.PLAYER_MAX_LIFE + 2;
		}

		return GlobalVariables.Instance.PLAYER_MAX_LIFE; 
	}

	public bool RegLifeAtPerfectDistance()
	{
		return (mLifePerkUnlockedLevel >= 3);
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
