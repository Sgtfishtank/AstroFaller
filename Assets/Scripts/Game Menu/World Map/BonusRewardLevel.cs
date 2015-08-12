﻿using UnityEngine;
using System.Collections;

public class BonusRewardLevel : LevelBase 
{
	public enum RewardType
	{
		Bolts,
		Crystals
	}
	public string mLevelName;
	public RewardType mRewardType;
	
	private TextMesh mTitleText;
	private TextMesh mRewardText;
	private	MeshRenderer mPictureImage;
	private	MeshRenderer mFrame;
	private bool mUnlocked;

	// Use this for initialization
	void Start () 
	{
	}	

	public override void Init()
	{
		mTitleText = transform.Find ("bonus reward/bonus text").GetComponent<TextMesh>();
		mRewardText = transform.Find ("bonus reward/reward text").GetComponent<TextMesh>();
		mPictureImage = transform.Find ("bonus reward/bonus_orb 2/bonus_orb").GetComponent<MeshRenderer>();
		mFrame = transform.Find ("bonus reward/bonus_frame").GetComponent<MeshRenderer>();
		mPictureImage.enabled = false;
		
		// add default
		if (mLevelName.Length < 1)
		{
			mLevelName = gameObject.name;
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		mTitleText.text = mLevelName;
		switch (mRewardType)
		{
		case RewardType.Bolts:
			mRewardText.text = GlobalVariables.Instance.BonusRewardBolts(mLevelName) + " bolts";
			break;
		case RewardType.Crystals:
			mRewardText.text = GlobalVariables.Instance.BonusRewardCrystals(mLevelName) + " crystals";
			break;
		default:
			print("Error RewardType in Update" + mRewardType);
			break;
		}
	}
	
	public override string LevelName ()
	{
		return mLevelName;
	}

	public override bool UnlockLevel()
	{
		if (!mUnlocked)
		{
			mUnlocked = true;
			mPictureImage.enabled = true;
			return true;
		}
		
		return false;
	}
	
	public override bool IsUnlocked()
	{
		return mUnlocked;
	}
	
	public override bool LockLevel()
	{
		if (mUnlocked)
		{
			mUnlocked = false;
			mPictureImage.enabled = false;
			return true;
		}
		
		return false;
	}
	
	public override void setFocusLevel (float focusLevel)
	{
		mFrame.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
		mPictureImage.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);

		TextMesh[] textMeshes = GetComponentsInChildren<TextMesh> ();
		for (int i = 0; i < textMeshes.Length; i++) 
		{
			textMeshes[i].color = new Color(mTitleText.color.r, mTitleText.color.g, mTitleText.color.b, focusLevel);
		}
	}
}