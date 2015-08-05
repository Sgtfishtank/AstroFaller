using UnityEngine;
using System.Collections;

public class WorldMapMenu : MonoBehaviour 
{
	public LevelBase[] mLevels;
	public float mScrollOffset;
	public float mLevelSize;
	public float mLevelSnapSpeed;
	public float mLevelScrollSpeed;

	public float mScrollValue;
	public int mCurrentLevelFocusIndex;
	private GameObject mLevelsScroller;

	// Use this for initialization
	public void Init() 
	{
		mLevelsScroller = transform.Find("Levels").gameObject;
		mLevels = mLevelsScroller.GetComponentsInChildren<LevelBase> ();
		mCurrentLevelFocusIndex = 0;
		
		setScrollerLevel(mScrollOffset);

		for (int i = 0; i < mLevels.Length; i++) 
		{
			mLevels[i].Init();
		}

		CheckLevels();
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
			mScrollValue = Mathf.Lerp (mScrollValue, mCurrentLevelFocusIndex * mLevelSize, mLevelSnapSpeed * Time.deltaTime);
		}
		else if (mScrollValue > ((mLevels.Length - 1) * mLevelSize)) 
		{
			mScrollValue = Mathf.Lerp (mScrollValue, mCurrentLevelFocusIndex * mLevelSize, mLevelSnapSpeed * Time.deltaTime);
		}

		mCurrentLevelFocusIndex = Mathf.RoundToInt (mScrollValue / mLevelSize);
		mCurrentLevelFocusIndex = Mathf.Clamp (mCurrentLevelFocusIndex, 0, (mLevels.Length - 1));
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ((Input.mouseScrollDelta.y > 0) || (Input.mouseScrollDelta.y < 0))
		{
			mCurrentLevelFocusIndex -= Mathf.RoundToInt(Input.mouseScrollDelta.y);
			mCurrentLevelFocusIndex = Mathf.Clamp (mCurrentLevelFocusIndex, 0, (mLevels.Length - 1));
		}
		else if (Input.GetKey(KeyCode.UpArrow))
		{
			ScrollLevels(mLevelScrollSpeed * Time.deltaTime);
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			ScrollLevels(-mLevelScrollSpeed * Time.deltaTime);
		}
		else
		{
			mScrollValue = Mathf.Lerp(mScrollValue, mCurrentLevelFocusIndex * mLevelSize, mLevelSnapSpeed * Time.deltaTime);
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

		setScrollerLevel(mScrollOffset + mScrollValue);

		for (int i = 0; i < mLevels.Length; i++) 
		{
			float diff = Mathf.Abs((i * mLevelSize) - mScrollValue) / mLevelSize;
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

	void setScrollerLevel (float scrollLevel)
	{
		mLevelsScroller.transform.localPosition = new Vector3 (mLevelsScroller.transform.localPosition.x, scrollLevel, mLevelsScroller.transform.localPosition.z);
	}
}
