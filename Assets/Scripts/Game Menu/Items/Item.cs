using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour 
{
	public enum ItemType
	{
		UnlimitedAir,
		Shockwave,
		BoltMagnet,
		ForceField,
		BoltMutliplier,
		RocketThrust
	}

	public ItemType mType;
	public string mItemName;
	public int mMaxItemLevel;
	public int mStartItemLevel;
	
	private	TextMesh mTitleText;
	private bool mUnlocked;
	private int mItemLevel;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool UnlockItem()
	{
		if (!mUnlocked)
		{
			mUnlocked = true;
			mItemLevel = Mathf.Min(mStartItemLevel, mMaxItemLevel);
			return true;
		}
		else if (mItemLevel < mMaxItemLevel)
		{
			mItemLevel++;
			return true;
		}
		
		return false;	
	}
	
	public bool IsUnlocked()
	{
		return mUnlocked;
	}

	public int ItemLevelUnlocked()
	{
		if (!mUnlocked)
		{
			return -1;
		}

		return mItemLevel;
	}
}
