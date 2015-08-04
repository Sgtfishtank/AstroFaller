using UnityEngine;
using System.Collections;

public class Perk : MonoBehaviour 
{
	public enum PerkType
	{
		Air,
		Life,
		Burst
	}
	
	public enum PerkPart
	{
		Main,
		Left,
		Right
	}

	public PerkType mType;
	public string mPerkName;

	private bool mMainUnlocked;
	private bool mLeftUnlocked;
	private bool mRightUnlocked;
	private	TextMesh mTitleText;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public bool UnlockPart(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			if (!mMainUnlocked)
			{
				mMainUnlocked = true;
				return true;
			}
			break;
		case PerkPart.Left:
			if (mMainUnlocked && (!mLeftUnlocked))
			{
				mLeftUnlocked = true;
				return true;
			}
			break;
		case PerkPart.Right:
			if (mMainUnlocked && (!mRightUnlocked))
			{
				mRightUnlocked = true;
				return true;
			}
			break;
		default:
			print("Error part in UnlockPart: " + perkPart);
			break;
		}

		return false;
	}

	public bool IsPartUnlocked(PerkPart perkPart)
	{
		switch (perkPart) 
		{
		case PerkPart.Main:
			return mMainUnlocked;
		case PerkPart.Left:
			return mMainUnlocked && mLeftUnlocked;
		case PerkPart.Right:
			return mMainUnlocked && mRightUnlocked;
		default:
			print("Error part in IsPartUnlocked: " + perkPart);
			break;
		}

		return false;
	}

}
