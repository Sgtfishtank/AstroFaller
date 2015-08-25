using UnityEngine;
using System.Collections;

public class WorldMapMenu : GameMenu 
{
	public LevelBase[] mLevels;
	//public GameObject mLevel;

	private float mScrollValue = 0;
	private int mCurrentLevelFocusIndex = 0;
	private LevelBase mCurrentLevel = null;
	private GameObject mLevelsScroller;
	private bool mFocused = false;
	private bool mLevelOpen = false;
	private bool mPlayLevelPhase = false;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (mPlayLevelPhase)
		{
			if (!MenuCamera.Instance.IsMoving())
			{
				mPlayLevelPhase = false;
				MainGameMenu.Instance.Disable();
				WorldGen.Instance.Enable("Level1");
			}
			else
			{
				Color fadeColor = Color.black;
				fadeColor.a = MenuCamera.Instance.MovingT();
				GUICanvas.Instance.SetFadeColor(fadeColor);
			}
		}
		else 
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
						ScrollLevels(GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * touch.deltaPosition.y * Time.deltaTime / touch.deltaTime);
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
				
				CloseLevels();
				MainGameMenu.Instance.UpdateMenusAndButtons();
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
				PlayLevel();
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
	}

	public override void Init() 
	{
		mLevelsScroller = transform.Find("Levels").gameObject;
		mLevels = mLevelsScroller.GetComponentsInChildren<LevelBase> ();

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

		enabled = false;
	}
	
	public override void Focus()
	{
		mFocused = true;
		enabled = true;
		CloseLevels ();
	}
	
	public override void Unfocus()
	{
		mFocused = false;
		enabled = false;
		CloseLevels ();
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
	}
	
	public override void UpdateMenusAndButtons ()
	{
		GUICanvas.Instance.showPlayLevelButton(mFocused && (!mPlayLevelPhase));
	}

	public LevelBase CurrentLevel()
	{
		return mCurrentLevel;
	}
	
	public override void BuyWithBolts()
	{
	}
	
	public override void BuyWithCrystals()
	{
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

		int last = mCurrentLevelFocusIndex;
		mCurrentLevelFocusIndex = Mathf.RoundToInt (mScrollValue / GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE);
		mCurrentLevelFocusIndex = Mathf.Clamp (mCurrentLevelFocusIndex, 0, (mLevels.Length - 1));
		if (last != mCurrentLevelFocusIndex) 
		{
			CloseLevels();
			MainGameMenu.Instance.UpdateMenusAndButtons();
		}
	}
	
	void setScrollerLevel (float scrollLevel)
	{
		mLevelsScroller.transform.localPosition = new Vector3 (mLevelsScroller.transform.localPosition.x, scrollLevel, mLevelsScroller.transform.localPosition.z);
	}

	public bool IsLevelOpen ()
	{
		return mLevelOpen;
	}

	public void PlayLevel ()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		if (mCurrentLevelFocusIndex < 0) 
		{
			print("ERROR nothing level in PlayLevel");
			return;
		}

		mCurrentLevel = mLevels [mCurrentLevelFocusIndex];
		if ((!mCurrentLevel.IsPlayable()) || (!mCurrentLevel.IsUnlocked()))
		{
			return;
		}

		if (!mLevelOpen)
		{
			OpenLevel((PlayableLevel)mCurrentLevel);
		}
		else
		{
			CloseLevels();
			StartPlayLevelPhase();
		}

		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void OpenLevel (PlayableLevel level)
	{
		mLevelOpen = true;
		level.Open();
	}

	public void CloseLevels()
	{
		mLevelOpen = false;
		for (int i = 0; i < mLevels.Length; i++) 
		{
			if (mLevels[i].IsPlayable())
			{
				((PlayableLevel)mLevels[i]).Close();
			}
		}
	}

	public bool IsTutorial ()
	{
		return (mCurrentLevelFocusIndex == 0);
	}

	public bool IsPlayLevelPhase()
	{
		return mPlayLevelPhase;
	}

	void StartPlayLevelPhase ()
	{
		mPlayLevelPhase = true;
		GUICanvas.Instance.ShowIconButtons(false);
		GUICanvas.Instance.showPlayLevelButton (false);
		MenuCamera.Instance.StartLevelZoom ();
	}
}
