using UnityEngine;
using System.Collections;

public class Level : LevelBase 
{
	public string mLevelName;
	
	private int mTotalDistance;
	private int mTotalBolts;
	private TextMesh mTitleText;
	private	TextMesh mTotalDistanceText;
	private	MeshRenderer mPictureImage;
	private MeshRenderer mFrame;
	private MeshRenderer mFrame2;
	private bool mUnlocked;

	// Use this for initialization
	void Start () 
	{
	}

	public override void Init()
	{
		mTitleText = transform.Find ("level/level name text").GetComponent<TextMesh> ();
		mTotalDistanceText = transform.Find ("level/top distance text").GetComponent<TextMesh> ();
		mPictureImage = transform.Find ("level/level picture").GetComponent<MeshRenderer> ();
		mFrame = transform.Find ("level/big_frame").GetComponent<MeshRenderer> ();
		mFrame2 = transform.Find ("level/small_frame 2/small_frame").GetComponent<MeshRenderer> ();
		
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
		mTotalDistanceText.text = "Max Distance\n" + mTotalDistance;
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
		mFrame2.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
		mPictureImage.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);

		TextMesh[] textMeshes = GetComponentsInChildren<TextMesh> ();
		for (int i = 0; i < textMeshes.Length; i++) 
		{
			textMeshes[i].color = new Color(mTitleText.color.r, mTitleText.color.g, mTitleText.color.b, focusLevel);
		}
	}

	public int TotalDistance ()
	{
		return mTotalDistance;
	}

	public int TotalBolts()
	{
		return mTotalBolts;
	}
}
