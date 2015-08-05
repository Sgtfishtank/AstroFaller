using UnityEngine;
using System.Collections;

public class TutorialLevel : LevelBase 
{
	public string mLevelName;

	private TextMesh mTitleText;
	private bool mUnlocked;
	private	MeshRenderer mPictureImage;
	private	MeshRenderer mFrame;

	// Use this for initialization
	void Start () 
	{
	}	
	
	public override void Init()
	{
		mTitleText = transform.Find ("tutorial/level name text").GetComponent<TextMesh> ();
		mPictureImage = transform.Find ("tutorial/level picture").GetComponent<MeshRenderer> ();
		mFrame = transform.Find ("tutorial/small_frame").GetComponent<MeshRenderer> ();
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
