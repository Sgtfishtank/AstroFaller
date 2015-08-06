using UnityEngine;
using System.Collections;

public class PlayerData : MonoBehaviour 
{
	public enum CashType
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

	public bool withdraw(int amount, CashType buyType)
	{
		if (amount < 0)
		{
			print("Error amount in withdraw " + amount);
			return false;
		}

		switch (buyType) 
		{
		case CashType.Bolts:
			if (mBolts >= amount)
			{
				mBolts -= amount;
				return true;
			}
			break;
		case CashType.Crystals:
			if (mCrystals >= amount)
			{
				mCrystals -= amount;
				return true;
			}
			break;
		default:
			print("error in withdraw " + buyType);
			break;
		}

		return false;
	}
	
	public bool deposit(int amount, CashType buyType)
	{
		if (amount < 0)
		{
			print("Error amount in deposit " + amount);
			return false;
		}

		switch (buyType) 
		{
		case CashType.Bolts:
			mBolts += amount;
			return true;
		case CashType.Crystals:
			mCrystals += amount;
			return true;
		default:
			print("error in deposit " + buyType);
			break;
		}
		
		return false;
	}
}
