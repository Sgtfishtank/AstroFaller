using UnityEngine;
using System.Collections;

public class BoltMagnet : Item 
{
	public GameObject mPrefab;
	public GameObject mObj;
	
	private bool mUnlocked;
	private int mItemLevel;
	public GameObject[] mObjParts;
	
	void Awake() 
	{
		mObj = GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		mObjParts = new GameObject[1 + GlobalVariables.Instance.ITEMS_MAX_LEVEL - GlobalVariables.Instance.ITEMS_START_LEVEL];
		mObjParts[0] = mObj.transform.Find("buy_orb " + 1).gameObject;
		mObjParts[1] = mObj.transform.Find("buy_orb " + 2).gameObject;
		mObjParts[2] = mObj.transform.Find("buy_orb " + 3).gameObject;
		mObjParts[0].SetActive(false);
		mObjParts[1].SetActive(false);
		mObjParts[2].SetActive(false);
	}

	// Use this for initialization
	void Start () 
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
			mObjParts[0].SetActive(true);
			return true;
		}
		else if (mItemLevel < GlobalVariables.Instance.ITEMS_MAX_LEVEL)
		{
			mItemLevel++;
			mObjParts[mItemLevel - GlobalVariables.Instance.ITEMS_START_LEVEL].SetActive(true);
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
		return GlobalVariables.Instance.BOLT_MAGNET_COST_BOLTS[mItemLevel];
	}
	
	public override int BuyCostCrystals()
	{
		return GlobalVariables.Instance.BOLT_MAGNET_COST_CRYSTALS[mItemLevel];
	}
	
	public override string BuyDescription()
	{
		return GlobalVariables.Instance.BOLT_MAGNET_DESCRIPTION;
	}
	
	public override string BuyCurrent()
	{
		return GlobalVariables.Instance.BOLT_MAGNET_LEVELS[mItemLevel] + GlobalVariables.Instance.BOLT_MAGNET_LEVELS_UNIT;
	}
	
	public override string BuyNext()
	{
		return GlobalVariables.Instance.BOLT_MAGNET_LEVELS[mItemLevel + 1] + GlobalVariables.Instance.BOLT_MAGNET_LEVELS_UNIT;
	}
}
