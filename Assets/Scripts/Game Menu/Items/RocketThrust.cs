using UnityEngine;
using System.Collections;

public class RocketThrust : Item 
{
	private bool mUnlocked;
	private int mItemLevel;
	
	// Use this for initialization
	void Start () 
	{
	}

	public override void Init()
	{

	}

	// Update is called once per frame
	void Update () 
	{
		
	}
	
	public override bool UnlockItem()
	{
		if (!mUnlocked)
		{
			mUnlocked = true;
			mItemLevel = Mathf.Min(GlobalVariables.Instance.ITEMS_START_LEVEL, GlobalVariables.Instance.ITEMS_MAX_LEVEL);
			return true;
		}
		else if (mItemLevel < GlobalVariables.Instance.ITEMS_MAX_LEVEL)
		{
			mItemLevel++;
			return true;
		}
		
		return false;	
	}
	
	public override bool IsUnlocked()
	{
		return mUnlocked;
	}
	
	public override int ItemLevelUnlocked()
	{
		if (!mUnlocked)
		{
			return -1;
		}
		
		return mItemLevel;
	}
	
	public override bool CanUnlockItem()
	{
		return ((!mUnlocked) || (mItemLevel < GlobalVariables.Instance.ITEMS_MAX_LEVEL));
	}
	
	public override int BuyCostBolts()
	{
		switch (mItemLevel) 
		{
		default:
			print ("Error perkPart in BuyCostBolts " + mItemLevel);
			break;
		}
		
		return -1;
	}
	
	public override int BuyCostCrystals()
	{
		switch (mItemLevel) 
		{
		default:
			print ("Error perkPart in BuyCostCrystals " + mItemLevel);
			break;
		}
		
		return -1;
	}
	
	public override string BuyDescription()
	{
		return "---";
	}
	
	public override string BuyCurrent()
	{
		return "---";
	}
	
	public override string BuyNext()
	{
		return "---";
	}
}
