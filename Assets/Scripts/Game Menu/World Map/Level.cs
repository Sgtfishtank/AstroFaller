using UnityEngine;
using System.Collections;

public class Level : PlayableLevel 
{
	public string mLevelName;
	public GameObject mLevelPrefab;
    public GameObject mPlayPrefab;
    public InGame.Level mLevel;
	
	private int mTotalDistance = 0;
	private int mTotalBolts = 0;
	private TextMesh mTitleText;
	private	TextMesh mTotalDistanceText;
	private	MeshRenderer mPictureImage;
	private MeshRenderer mFrame;
	private MeshRenderer mFrame2;
	private bool mUnlocked;
	private	ButtonManager mPlayButton;
	private	GameObject mLevelObj;
	private TextMesh[] mTextMeshes;
    private MeshRenderer[] mMeshRenders;
    private float mFocusLevel = -1;

	void Awake () 
	{
		mTotalDistance = -1;

        mLevelObj = GlobalVariables.Instance.Instanciate(mLevelPrefab, transform, 1);
        mLevelObj.transform.name = "level";
		
		GameObject mPlayButton2 = GlobalVariables.Instance.Instanciate (mPlayPrefab, transform, 1);
		mPlayButton2.transform.name = "PlayLevelButton";

        mTitleText = mLevelObj.transform.Find("level name text").GetComponent<TextMesh>();
        mTotalDistanceText = mLevelObj.transform.Find("top distance text").GetComponent<TextMesh>();
        mPictureImage = mLevelObj.transform.Find("level picture").GetComponent<MeshRenderer>();
        mFrame = mLevelObj.transform.Find("big_frame").GetComponent<MeshRenderer>();
        mFrame2 = mLevelObj.transform.Find("small_frame 2/small_frame").GetComponent<MeshRenderer>();
		
		mTextMeshes = GetComponentsInChildren<TextMesh> ();
		mMeshRenders = GetComponentsInChildren<MeshRenderer> ();

		// add default
		if (mLevelName.Length < 1)
		{
			mLevelName = gameObject.name;
		}

        mPlayButton = ButtonManager.CreateButton(gameObject, "PlayLevelButton");

		mPictureImage.enabled = false;
	}

	// Use this for initialization
	void Start () 
	{
        GUICanvasBase gui = MenuGUICanvas.Instance.WorldMapMenu();
        mPlayButton.LoadButtonPress("PlayLevelButton", gui);
		mPlayButton.mObj.SetActive (false);
		SetTotalDistance(0);
	}

	public override void Init()
	{
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
	
	public override InGame.Level GetLevel()
	{
        return mLevel;
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
        mPlayButton.mObj.SetActive(true);
	}
	
	public override void Close()
	{
        mPlayButton.mObj.SetActive(false);
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
        if (mFocusLevel == focusLevel)
        {
            return;
        }

        if (Mathf.Abs(mFocusLevel - focusLevel) < 0.02f)
        {
            return;
        }
        mFocusLevel = focusLevel;

        Vector3 mFocusOffset = new Vector3(0, 0, GlobalVariables.Instance.LEVELS_FOCUS_ZOOM * focusLevel);
        mFrame.transform.localPosition = mFocusOffset;
        mFrame2.transform.localPosition = mFocusOffset;
        mPictureImage.transform.localPosition = mFocusOffset;
		mPlayButton.SetBaseOffset(mFocusOffset);

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

	public void SetTotalDistance(int totalDistance)
	{
		// avoid string allocations
		if (mTotalDistance != totalDistance) 
		{
			mTotalDistance = totalDistance;
			mTotalDistanceText.text = totalDistance.ToString();
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
