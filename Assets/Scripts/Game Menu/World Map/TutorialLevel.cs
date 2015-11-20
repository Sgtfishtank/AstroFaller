using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialLevel : PlayableLevel 
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
	private TextMesh[] mTextMeshes;
	private MeshRenderer[] mMeshRenders;

	void Awake () 
	{
		mTutorial = GlobalVariables.Instance.Instanciate (mLevelPrefab, transform, 1);
		mTutorial.transform.name = "tutorial";
		
		mPlayButton = GlobalVariables.Instance.Instanciate (mPlayPrefab, transform, 0.75f);
		mPlayButton.transform.name = "PlayLevelButton";
		
		mTitleText = mTutorial.transform.Find ("level name text").GetComponent<TextMesh> ();
		mPictureImage = mTutorial.transform.Find ("level picture").GetComponent<MeshRenderer> ();
		mFrame = mTutorial.transform.Find ("small_frame").GetComponent<MeshRenderer> ();
		
		mTextMeshes = GetComponentsInChildren<TextMesh> ();
		mMeshRenders = GetComponentsInChildren<MeshRenderer> ();

		mPlayButton.SetActive (false);
		
		mPictureImage.enabled = false;
		
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
	}

	public override int GetLevelIndex ()
	{
		switch (mLevelName) 
		{
		case "Asteroid Belt":
			return 1;
		}

		return -1;
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
		
		mPlayButton.transform.localPosition += GUICanvas.Instance.MenuGUICanvas().PlayButton().PositionOffset();
		mPlayButton.transform.localScale = Vector3.one * 0.84f * GUICanvas.Instance.MenuGUICanvas().PlayButton().ScaleFactor();

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
