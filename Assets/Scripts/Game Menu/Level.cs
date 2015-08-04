using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour 
{
	public enum LevelType
	{
		Tutorial,
		Level,
		BonusReward
	}
	public LevelType mType;
	public string mLevelName;
	public int mTotalDistance;

	private bool mUnlocked;
	private	TextMesh mTitleText;
	private	TextMesh mTotalDistanceText;

	// Use this for initialization
	void Start () 
	{
		mUnlocked = false;

		// TODO fix
		//mTitleText = transform.Find ("Title").GetComponent<TextMesh> ();
		//mTotalDistanceText = transform.Find ("Distance").GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// TODO fix
		//mTitleText.text = mLevelName;
		//mTotalDistanceText.text = "Max Distance\n" + mTotalDistance;
	}

	public bool Unlock()
	{
		if (!mUnlocked)
		{
			mUnlocked = true;
			return true;
		}

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

		return false;
	}
}
