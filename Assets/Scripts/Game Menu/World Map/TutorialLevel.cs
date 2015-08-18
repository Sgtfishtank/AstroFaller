using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialLevel : LevelBase 
{
	public string mLevelName;
	public GameObject mPrefab;

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
		GameObject gab = GameObject.Instantiate (mPrefab);
		gab.transform.parent = transform;
		gab.transform.localPosition = Vector3.zero;
		gab.transform.localRotation = Quaternion.identity;
		gab.transform.localScale = Vector3.one;
		gab.transform.name = "tutorial";

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
	
	public override string LevelName ()
	{
		return mLevelName;
	}
	
	public override bool IsPlayable()
	{
		return true;
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
