﻿using UnityEngine;
using System.Collections;

public class WorldMapMenu : GameMenu 
{
	public LevelBase[] mLevels;

	private float mScrollValue;
	private int mCurrentLevelFocusIndex;
	private PlayableLevel mCurrentLevel;
	private GameObject mLevelsScroller;
	private bool mFocused;

	private bool mPlayLevelPhase;

	void Awake () 
	{
		mLevelsScroller = transform.Find("Levels").gameObject;
		mLevels = mLevelsScroller.GetComponentsInChildren<LevelBase> ();

		setScrollerLevel(GlobalVariables.Instance.WORLD_MAP_SCROLL_OFFSET);
	}

	// Use this for initialization
	void Start ()
    {
		CheckLevels();
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
                PlayerData.LoadScene(PlayerData.Scene.IN_GAME);
			}
			else
			{
				Color fadeColor = Color.black;
				fadeColor.a = MenuCamera.Instance.MovingT();
				MenuGUICanvas.Instance.SetFadeColor(fadeColor);
			}
		}
		else 
		{
			if ((Input.touchCount > 0) && mFocused)
			{
				Touch touch = Input.touches[0];
				//foreach (Touch touch in Input.touches)
				//{
					switch (touch.phase)
					{
					case TouchPhase.Began:
						break;
					case TouchPhase.Moved:
						ScrollLevels(GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * touch.deltaPosition.y * Time.deltaTime / (touch.deltaTime + 0.01f));
						break;
					case TouchPhase.Canceled:
						break;
					case TouchPhase.Ended:
						break;
					}
				//}
			}
			else if (((Input.mouseScrollDelta.y > 0) || (Input.mouseScrollDelta.y < 0)) && mFocused)
			{
				mCurrentLevelFocusIndex -= Mathf.RoundToInt(Input.mouseScrollDelta.y);
				mCurrentLevelFocusIndex = Mathf.Clamp (mCurrentLevelFocusIndex, 0, (mLevels.Length - 1));
				
				CloseLevels();
				MainGameMenu.Instance.UpdateMenusAndButtons();
			}
            else if (Input.GetKey(KeyCode.UpArrow) && mFocused)
			{
				ScrollLevels(GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * 1000 * Time.deltaTime);
			}
            else if (Input.GetKey(KeyCode.DownArrow) && mFocused)
			{
				ScrollLevels(-GlobalVariables.Instance.WORLD_MAP_LEVELS_SCROLL_SPEED * 1000 * Time.deltaTime);
			}
			else 
			{
				mScrollValue = Mathf.Lerp(mScrollValue, mCurrentLevelFocusIndex * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE, GlobalVariables.Instance.WORLD_MAP_LEVELS_SNAP_SPEED * Time.deltaTime);
			}
			
			if (Input.GetKeyDown(KeyCode.U) && mFocused)
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

			mScrollValue = Mathf.Clamp(mScrollValue, -1 * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE, mLevels.Length * GlobalVariables.Instance.WORLD_MAP_LEVELS_SIZE);
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

	public override void Focus()
    {
        if (!mFocused)
        {
            CloseLevels();
        }

		mFocused = true;
	}
	
	public override void Unfocus()
	{
        if (mFocused)
        {
            CloseLevels();
        }

		mFocused = false;
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
	}
	
	public override void UpdateMenusAndButtons ()
	{
        MenuGUICanvas.Instance.WorldMapMenu().ShowPlayLevelButton(mFocused && (!mPlayLevelPhase));
        MenuGUICanvas.Instance.ShowWorldMapButtons(mFocused && (!mPlayLevelPhase));
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
		return (mCurrentLevel != null);
	}

	public void PlayLevel ()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		if (mCurrentLevelFocusIndex < 0) 
		{
			print("ERROR nothing level in PlayLevel");
			return;
		}

		LevelBase level = mLevels [mCurrentLevelFocusIndex];

		if ((!level.IsPlayable()) || (!level.IsUnlocked()))
        {
			print("Not playable");
			return;
		}

		if (!IsLevelOpen())
        {
			OpenLevel((PlayableLevel)level);
		}
		else
		{
			StartPlayLevelPhase();
			CloseLevels();
		}

		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void OpenLevel (PlayableLevel level)
	{
		mCurrentLevel = level;
		level.Open();
	}

	public void CloseLevels()
	{
		mCurrentLevel = null;
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
        PlayerData.Instance.LevelToLoad = mCurrentLevel.GetLevel();
		MenuGUICanvas.Instance.ShowIconButtons(false);
		MenuGUICanvas.Instance.WorldMapMenu().ShowPlayLevelButton(false);
		MenuCamera.Instance.StartLevelZoom ();
	}
    
    public override void Deselect()
    {
        if (mPlayLevelPhase)
	    {
            MenuCamera.Instance.Skip();
	    }
    }
}
