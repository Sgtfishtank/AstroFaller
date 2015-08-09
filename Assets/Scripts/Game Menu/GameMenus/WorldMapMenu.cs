using UnityEngine;
using System.Collections;

public class WorldMapMenu : GameMenu 
{
	public LevelBase[] mLevels;

	private float mScrollValue;
	private int mCurrentLevelFocusIndex;
	private GameObject mLevelsScroller;
	private bool mFocused;

	// Use this for initialization
	void Start () 
	{
	}

	public override void Init() 
	{
		mLevelsScroller = transform.Find("Levels").gameObject;
		mLevels = mLevelsScroller.GetComponentsInChildren<LevelBase> ();
		mCurrentLevelFocusIndex = 0;
		
		setScrollerLevel(GlobalVariables.Instance.WORLD_MAP_SCROLL_OFFSET);

		for (int i = 0; i < mLevels.Length; i++) 
		{
			mLevels[i].Init();

			UnlockCriteria[] criterias = mLevels[i].GetComponents<UnlockCriteria>();
			for (int j = 0; j < criterias.Length; j++) 
			{
				criterias[j].Init();
			}
		}

		CheckLevels();
		
		mFocused = false;
		enabled = false;
	}
	
	public override void Focus()
	{
		mFocused = true;
		enabled = true;
	}
	
	public override void Unfocus()
	{
		mFocused = false;
		enabled = false;
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
	}

	void CheckLevels ()
	{
		for (int i = 0; i < mLevels.Length; i++) 
		{
			LevelBase mLevel = mLevels[i];

			CheckLevel(mLevel);
		}
	}

	void CheckLevel (LevelBase level)
	{
		bool allCriteraMet = true;
		
		UnlockCriteria[] mUnlockCriterias = level.GetComponents<UnlockCriteria>();
		for (int j = 0; j < mUnlockCriterias.Length; j++) 
		{
			UnlockCriteria mUnlockCriteria = mUnlockCriterias[j];
			if (!mUnlockCriteria.CriteriaMet())
			{
				allCriteraMet = false;
			}
		}
		
		if (allCriteraMet)
		{
			level.UnlockLevel();
		}
		else
		{
			level.LockLevel();
		}
	}

	void ScrollLevels(float scrollAmount)
	{
		mScrollValue += scrollAmount;
		if (mScrollValue < 0) 
		{
			mScrollValue = Mathf.Lerp (mScrollValue, mCurrentLevelFocusIndex * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE, GlobalVariables.Instance.WORLD_MAP_LEVELS_SNAP_SPEED * Time.deltaTime);
		}
		else if (mScrollValue > ((mLevels.Length - 1) * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE)) 
		{
			mScrollValue = Mathf.Lerp (mScrollValue, mCurrentLevelFocusIndex * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE, GlobalVariables.Instance.WORLD_MAP_LEVELS_SNAP_SPEED * Time.deltaTime);
		}

		mCurrentLevelFocusIndex = Mathf.RoundToInt (mScrollValue / GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE);
		mCurrentLevelFocusIndex = Mathf.Clamp (mCurrentLevelFocusIndex, 0, (mLevels.Length - 1));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.touchCount > 0)
		{
			foreach (Touch touch in Input.touches)
			{
				switch (touch.phase)
				{
				case TouchPhase.Began:
					break;
				case TouchPhase.Moved:
					if (touch.deltaPosition.y > 0.1f)
					{
						ScrollLevels(GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * Time.deltaTime);
						print("+");
					}
					else if (touch.deltaPosition.y < 0.1f)
					{
						print("-");
						ScrollLevels(-GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * Time.deltaTime);
					}
					break;
				case TouchPhase.Canceled:
					break;
				case TouchPhase.Ended:
					break;
				}
			}
		}

		if ((Input.mouseScrollDelta.y > 0) || (Input.mouseScrollDelta.y < 0))
		{
			mCurrentLevelFocusIndex -= Mathf.RoundToInt(Input.mouseScrollDelta.y);
			mCurrentLevelFocusIndex = Mathf.Clamp (mCurrentLevelFocusIndex, 0, (mLevels.Length - 1));
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			ScrollLevels(GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			ScrollLevels(-GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * Time.deltaTime);
		}
		else 
		{
			mScrollValue = Mathf.Lerp(mScrollValue, mCurrentLevelFocusIndex * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE, GlobalVariables.Instance.WORLD_MAP_LEVELS_SNAP_SPEED * Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.U))
		{
			if (mLevels[mCurrentLevelFocusIndex].IsUnlocked()) 
			{
				mLevels[mCurrentLevelFocusIndex].LockLevel();
			}
			else
			{
				mLevels[mCurrentLevelFocusIndex].UnlockLevel();
			}
		}
		else if (Input.GetKeyDown(KeyCode.Return))
		{
			Application.LoadLevel("Level" + mCurrentLevelFocusIndex);
		}

		setScrollerLevel(GlobalVariables.Instance.WORLD_MAP_SCROLL_OFFSET + mScrollValue);

		for (int i = 0; i < mLevels.Length; i++) 
		{
			float diff = Mathf.Abs((i * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE) - mScrollValue) / GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE;
			if (diff < 1)
			{
				mLevels[i].setFocusLevel(1 - diff);
			}
			else
			{
				mLevels[i].setFocusLevel(0);
			}
		}
	}
	
	public override void BuyWithBolts()
	{
	}
	
	public override void BuyWithCrystals()
	{
	}

	void setScrollerLevel (float scrollLevel)
	{
		mLevelsScroller.transform.localPosition = new Vector3 (mLevelsScroller.transform.localPosition.x, scrollLevel, mLevelsScroller.transform.localPosition.z);
	}
}
