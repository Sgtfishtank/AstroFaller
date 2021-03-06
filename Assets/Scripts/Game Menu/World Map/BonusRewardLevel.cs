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
	public GameObject mPrefab;
	
	public TextMesh mTitleText;
	private TextMesh mRewardText;
	private	MeshRenderer mPictureImage;
	private	MeshRenderer mFrame;
	private bool mUnlocked;
	private int mReward;
	private TextMesh[] mTextMeshes;
    private MeshRenderer[] mMeshRenders;
    private float mFocusLevel = -1;

	void Awake()
	{
		mReward = -1;
		GameObject gab = GameObject.Instantiate (mPrefab);
		gab.transform.parent = transform;
		gab.transform.localPosition = Vector3.zero;
		gab.transform.localRotation = Quaternion.identity;
		gab.transform.localScale = Vector3.one;
		gab.transform.name = "bonus reward";
		
		mTitleText = transform.Find ("bonus reward/bonus text").GetComponent<TextMesh>();
		mRewardText = transform.Find ("bonus reward/reward text").GetComponent<TextMesh>();
		mPictureImage = transform.Find ("bonus reward/bonus_orb 2/bonus_orb").GetComponent<MeshRenderer>();
		mFrame = transform.Find ("bonus reward/bonus_frame").GetComponent<MeshRenderer>();
		mPictureImage.enabled = false;
		
		mTextMeshes = GetComponentsInChildren<TextMesh> ();
		mMeshRenders = GetComponentsInChildren<MeshRenderer> ();

		// add default
		if (mLevelName.Length < 1)
		{
			mLevelName = gameObject.name;
		}
	}

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
		mTitleText.text = mLevelName;

		switch (mRewardType)
		{
		case RewardType.Bolts:
			// avoid string allocations
			if (mReward != GlobalVariables.Instance.BonusRewardBolts(mLevelName)) 
			{
				mReward = GlobalVariables.Instance.BonusRewardBolts(mLevelName);
				mRewardText.text = GlobalVariables.Instance.BonusRewardBolts(mLevelName) + "";
			}
			break;
		case RewardType.Crystals:
			// avoid string allocations
			if (mReward != GlobalVariables.Instance.BonusRewardCrystals(mLevelName)) 
			{
				mReward = GlobalVariables.Instance.BonusRewardCrystals(mLevelName);
				mRewardText.text = GlobalVariables.Instance.BonusRewardCrystals(mLevelName) + "";
			}
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
	
	public override bool IsPlayable()
	{
		return false;
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
        if (mFocusLevel == focusLevel)
        {
            return;
        }

        if (Mathf.Abs(mFocusLevel - focusLevel) < 0.02f)
        {
            return;
        }
        mFocusLevel = focusLevel;

		Vector3 mFocusOffset = new Vector3(0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
        mFrame.transform.localPosition = mFocusOffset;
        mPictureImage.transform.localPosition = mFocusOffset;
		
		for (int i = 0; i < mTextMeshes.Length; i++)
        {
			Color x = mTextMeshes[i].color;
			x.a = focusLevel;
			mTextMeshes[i].color = x;
		}
		
		for (int i = 0; i < mMeshRenders.Length; i++) 
		{
			Color x = mMeshRenders[i].material.color;
			x.a = focusLevel;
			mMeshRenders[i].material.color = x;
		}
	}
}
