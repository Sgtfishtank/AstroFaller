﻿using UnityEngine;
using System.Collections;

public class BurstPerk : Perk 
{
	public string mPerkName;
	
	private bool mMainUnlocked;
	private bool mLeftUnlocked;
	private bool mRightUnlocked;
	private	TextMesh mTitleText;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	public override void Init()
	{
		if (mPerkName.Length < 1)
		{
			mPerkName = gameObject.name;
		}
		
		mTitleText = transform.Find ("Burst/Burst+ text").GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTitleText.text = mPerkName;
	}

	public override bool UnlockPart(PerkPart perkPart)
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
	
	public override bool IsPartUnlocked(PerkPart perkPart)
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
