using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	public enum LevelType
	{
		Tutorial,
		Level,
		BonusReward
	}
	public LevelType mLevelType;
	public string mLevelName;
	public int mTotalDistance;

	private bool mUnlocked;
	private	TextMesh mTitleText;
	private	TextMesh mTotalDistanceText;

	// Use this for initialization
	void Start () 
	{
		mUnlocked = false;

		mTitleText = transform.Find ("Title").GetComponent<TextMesh> ();
		mTotalDistanceText = transform.Find ("Dsitance").GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	public bool Unlock()
	{
		if (!mUnlocked)
		{
			mUnlocked = true;
			return true;
		}

		mUnlocked = true;
		return false;
	}

	public bool IsUnlocked()
	{
		return mUnlocked;
	}

	public bool Lock()
	{
		if (mUnlocked)
		{
			mUnlocked = false;
			return true;
		}
		
		mUnlocked = true;
		return false;
	}
}
