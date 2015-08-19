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
	public GameObject mPrefab;
	
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
			if (!mUnlocked)
			{
				mRewardText.text = "Locked. Requierd distance:\n" + GlobalVariables.Instance.DistanceCritera(mLevelName);
			}
			else 
			{
				mRewardText.text = GlobalVariables.Instance.BonusRewardBolts(mLevelName) + " bolts rewarded";
			}
			break;
		case RewardType.Crystals:
			if (!mUnlocked)
			{
				mRewardText.text = "Locked. Requierd distance: " + GlobalVariables.Instance.DistanceCritera(mLevelName);
			}
			else 
			{
				mRewardText.text = GlobalVariables.Instance.BonusRewardCrystals(mLevelName) + " crystals rewarded";
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
	
	public override void Open()
	{
	}
	
	public override void Close()
	{
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
