﻿using UnityEngine;
using System.Collections;

public class UnlimitedAirItem : Item 
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
		return GlobalVariables.Instance.UNLIMITED_AIR_COST_BOLTS[mItemLevel];
	}
	
	public override int BuyCostCrystals()
	{
		return GlobalVariables.Instance.UNLIMITED_AIR_COST_CRYSTALS[mItemLevel];
	}
	
	public override string BuyDescription()
	{
		return GlobalVariables.Instance.UNLIMITED_AIR_DESCRIPTION;
	}
	
	public override string BuyCurrent()
	{
		return GlobalVariables.Instance.UNLIMITED_AIR_LEVELS[mItemLevel] + GlobalVariables.Instance.UNLIMITED_AIR_LEVELS_UNIT;
	}
	
	public override string BuyNext()
	{
		return GlobalVariables.Instance.UNLIMITED_AIR_LEVELS[mItemLevel + 1] + GlobalVariables.Instance.UNLIMITED_AIR_LEVELS_UNIT;
	}
}