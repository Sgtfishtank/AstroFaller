using UnityEngine;
using System.Collections;

public class ForceField : Item 
{
	public GameObject mPrefab;

	private GameObject mObj;
	private bool mUnlocked;
	private int mItemLevel;
	private GameObject[] mObjParts;
	
	void Awake() 
	{
		mObj = GlobalVariables.Instance.Instanciate (mPrefab, transform, 1);
		mObjParts = new GameObject[3];
		for (int i = 0; i < mObjParts.Length; i++) 
		{
			mObjParts[i] = mObj.transform.Find("buy_orb " + (i + 1)).gameObject;
			mObjParts[i].SetActive(false);
		}

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
		return GlobalVariables.Instance.FORCE_FIELD_COST_BOLTS[mItemLevel];
	}
	
	public override int BuyCostCrystals()
	{
		return GlobalVariables.Instance.FORCE_FIELD_COST_CRYSTALS[mItemLevel];
	}
	
	public override string BuyDescription()
	{
		return GlobalVariables.Instance.FORCE_FIELD_DESCRIPTION;
	}
	
	public override string BuyCurrent()
	{
		return GlobalVariables.Instance.FORCE_FIELD_LEVELS[mItemLevel] + GlobalVariables.Instance.FORCE_FIELD_LEVELS_UNIT;
	}
	
	public override string BuyNext()
	{
		return GlobalVariables.Instance.FORCE_FIELD_LEVELS[mItemLevel + 1] + GlobalVariables.Instance.FORCE_FIELD_LEVELS_UNIT;
	}
}
