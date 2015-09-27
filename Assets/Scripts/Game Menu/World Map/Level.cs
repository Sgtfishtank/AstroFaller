using UnityEngine;
using System.Collections;

public class Level : PlayableLevel 
{
	public string mLevelName;
	public GameObject mLevelPrefab;
	public GameObject mPlayPrefab;
	
	private int mTotalDistance = 0;
	private int mTotalBolts = 0;
	private TextMesh mTitleText;
	private	TextMesh mTotalDistanceText;
	private	MeshRenderer mPictureImage;
	private MeshRenderer mFrame;
	private MeshRenderer mFrame2;
	private bool mUnlocked;
	private	GameObject mPlayButton;
	private	GameObject mLevel;
	
	void Awake () 
	{
		mLevel = GlobalVariables.Instance.Instanciate (mLevelPrefab, transform, 1);
		mLevel.transform.name = "level";
		
		mPlayButton = GlobalVariables.Instance.Instanciate (mPlayPrefab, transform, 1);
		mPlayButton.transform.name = "PlayLevelButton";
		
		mTitleText = mLevel.transform.Find ("level name text").GetComponent<TextMesh> ();
		mTotalDistanceText = mLevel.transform.Find ("top distance text").GetComponent<TextMesh> ();
		mPictureImage = mLevel.transform.Find ("level picture").GetComponent<MeshRenderer> ();
		mFrame = mLevel.transform.Find ("big_frame").GetComponent<MeshRenderer> ();
		mFrame2 = mLevel.transform.Find ("small_frame 2/small_frame").GetComponent<MeshRenderer> ();
	}

	// Use this for initialization
	void Start () 
	{
	}

	public override void Init()
	{
		mPlayButton.SetActive (false);

		
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

		//if (!mUnlocked)
		{
			//mTotalDistanceText.text = "Locked. Required distance:\n" + GlobalVariables.Instance.DistanceCritera(mLevelName);
		}
		//else
		{
			mTotalDistanceText.text = mTotalDistance.ToString();
		}
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
	
	public override void Open()
	{
		mPlayButton.SetActive (true);
	}
	
	public override void Close()
	{
		mPlayButton.SetActive (false);
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

		Close ();
		return false;
	}
	
	public override void setFocusLevel (float focusLevel)
	{
		mFrame.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
		mFrame2.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
		mPictureImage.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
		mPlayButton.transform.localPosition = new Vector3 (0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);

		mPlayButton.transform.localPosition += GUICanvas.Instance.MenuGUICanvas().PlayButton().PositionOffset();
		mPlayButton.transform.localScale = Vector3.one * GUICanvas.Instance.MenuGUICanvas().PlayButton().ScaleFactor();

		TextMesh[] textMeshes = GetComponentsInChildren<TextMesh> ();
		for (int i = 0; i < textMeshes.Length; i++) 
		{
			Color x = textMeshes[i].color;
			x.a = focusLevel;
			textMeshes[i].color = x;
		}
		
		MeshRenderer[] rextMeshes = GetComponentsInChildren<MeshRenderer> ();
		for (int i = 0; i < rextMeshes.Length; i++) 
		{
			Color x = rextMeshes[i].material.color;
			x.a = focusLevel;
			rextMeshes[i].material.color = x;
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
