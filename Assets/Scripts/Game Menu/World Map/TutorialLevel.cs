using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialLevel : LevelBase 
{
	public string mLevelName;
	public GameObject mLevelPrefab;
	public GameObject mPlayPrefab;

	private TextMesh mTitleText;
	private bool mUnlocked;
	private	MeshRenderer mPictureImage;
	private	MeshRenderer mFrame;
	private	GameObject mPlayButton;
	private	GameObject mTutorial;

	// Use this for initialization
	void Start () 
	{
	}	
	
	public override void Init()
	{
		mTutorial = GlobalVariables.Instance.Instanciate (mLevelPrefab, transform, 1);
		mTutorial.transform.name = "tutorial";

		mPlayButton = GlobalVariables.Instance.Instanciate (mPlayPrefab, transform, 0.75f);
		mPlayButton.transform.name = "PlayLevelButton";
		mPlayButton.SetActive (false);

		mTitleText = mTutorial.transform.Find ("level name text").GetComponent<TextMesh> ();
		mPictureImage = mTutorial.transform.Find ("level picture").GetComponent<MeshRenderer> ();
		mFrame = mTutorial.transform.Find ("small_frame").GetComponent<MeshRenderer> ();
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
	
	public override void Open()
	{
		mPlayButton.SetActive (true);
	}
	
	public override void Close()
	{
		mPlayButton.SetActive (false);
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
		mPlayButton.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);

		TextMesh[] textMeshes = GetComponentsInChildren<TextMesh> ();
		for (int i = 0; i < textMeshes.Length; i++) 
		{
			textMeshes[i].color = new Color(mTitleText.color.r, mTitleText.color.g, mTitleText.color.b, focusLevel);
		}
	}
}
