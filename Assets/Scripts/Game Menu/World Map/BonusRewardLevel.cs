using UnityEngine;
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
	public int mRewardAmount;
	
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
			mRewardText.text = mRewardAmount + " bolts";
			break;
		case RewardType.Crystals:
			mRewardText.text = mRewardAmount + " crystals";
			break;
		default:
			print("Error RewardType in Update" + mRewardType);
			break;
		}
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
		mFrame.transform.localPosition = new Vector3 (0, 0, 100 * focusLevel);
		mPictureImage.transform.localPosition = new Vector3 (0, 0, 100 * focusLevel);
	}
}
