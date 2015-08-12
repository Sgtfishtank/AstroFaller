using UnityEngine;
using System.Collections;

public class MenuCamera : MonoBehaviour 
{
	// snigleton
	private static MenuCamera instance = null;
	public static MenuCamera Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("Menu Camera");
				instance = thisObject.GetComponent<MenuCamera>();
			}
			return instance;
		}
	}

	public enum MoveType
	{
		Linerar,
		IsoscelesTriangle,
		Hyperbole,
	}

	public MoveType mCameraMoveType;
	public bool mUseSmoothStep;

	private Vector3 mStartMenuPosition;
	private Vector3 mTargetMenuPosition;
	private bool mMoving;
	private float mMovingT;

	private PopupBuyMenu mPopupBuyMenu;

	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	private GameObject mHelpMenu;
	private GameObject mOptionsMenu;
	private GameObject mWorldMapButton;
	
	// Use this for initialization
	void Start () 
	{
		mHelpMenu = transform.Find("Help").gameObject;
		mOptionsMenu = transform.Find("Options").gameObject;
		mWorldMapButton = transform.Find("WorldMapButton").gameObject;
		mPopupCraftingMenu = transform.Find("PopupCraftingMenu").gameObject;
		mPopupAchievementsMenu = transform.Find("PopupAchievementsMenu").gameObject;
		mMoving = false;

		mPopupBuyMenu = transform.Find("PopupBuyMenu").GetComponent<PopupBuyMenu>();
		mPopupBuyMenu.Init();

		HideHelpMenu();
		HideOptionsMenu();
		HideBackButton();
		HidePopupCraftingMenu();
		HidePopupAchievementsMenu();
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 mCameraOffset = GlobalVariables.Instance.MAIN_CAMERA_OFFSET;
		if (mMoving) 
		{
			mMovingT = Mathf.Clamp01(mMovingT + (Time.deltaTime * GlobalVariables.Instance.MAIN_CAMERA_OFFSET_MOVE_SPEED));

			float movingTime = mMovingT;
			if (mUseSmoothStep)
			{
				movingTime = Mathf.SmoothStep(0, 1, mMovingT);
			}

			float dist = Vector3.Distance(mStartMenuPosition, mTargetMenuPosition);
			Vector3 midPoint = ((mStartMenuPosition + mTargetMenuPosition) * 0.5f) + new Vector3(0, 0, dist) * GlobalVariables.Instance.MAIN_CAMERA_MOVE_ZOOM_OUT_FACTOR;

			switch (mCameraMoveType) 
			{
			case MoveType.IsoscelesTriangle:
				if (mMovingT <= 0.5f)
				{
					transform.position = Vector3.Lerp(mStartMenuPosition + mCameraOffset, midPoint + mCameraOffset, movingTime * 2);
				}
				else
				{
					transform.position = Vector3.Lerp(midPoint + mCameraOffset, mTargetMenuPosition + mCameraOffset, (movingTime - 0.5f) * 2);
				}
				break;
			case MoveType.Hyperbole:
				if (mMovingT <= 0.5f)
				{
					float movingTimePart = movingTime * 2;
					float zdiff = Mathf.Abs(midPoint.z - mStartMenuPosition.z);
					//float ydiff = Mathf.Abs(midPoint.y - mStartMenuPosition.y);
					//float xdiff = Mathf.Abs(midPoint.x - mStartMenuPosition.x);
					float x = Mathf.Lerp(mStartMenuPosition.x, midPoint.x, movingTimePart);
					float y = Mathf.Lerp(mStartMenuPosition.y, midPoint.y, movingTimePart);
					float z = mStartMenuPosition.z + zdiff - (zdiff * Mathf.Pow(1 - movingTimePart, 2));

					// prevent other dimentions from appearing
					if (midPoint.z < mStartMenuPosition.z)
					{
						z = mStartMenuPosition.z - zdiff + (zdiff * Mathf.Pow(1 - movingTimePart, 2));
					}

					transform.position = new Vector3(x, y, z) + mCameraOffset;
				}
				else
				{
					float movingTimePart = (movingTime - 0.5f) * 2;
					float zdiff = Mathf.Abs(midPoint.z - mTargetMenuPosition.z);
					//float ydiff = Mathf.Abs(midPoint.y - mTargetMenuPosition.y);
					//float xdiff = Mathf.Abs(midPoint.x - mTargetMenuPosition.x);
					float x = Mathf.Lerp(midPoint.x, mTargetMenuPosition.x, movingTimePart);
					float y = Mathf.Lerp(midPoint.y, mTargetMenuPosition.y, movingTimePart);
					float z = midPoint.z - (zdiff * Mathf.Pow(movingTimePart, 2));
					
					// prevent other dimentions from appearing
					if (midPoint.z < mTargetMenuPosition.z)
					{
						z = midPoint.z + (zdiff * Mathf.Pow(movingTimePart, 2));
					}

					transform.position = new Vector3(x, y, z) + mCameraOffset;
				}
				break;
			default: case MoveType.Linerar:
				transform.position = Vector3.Lerp(mStartMenuPosition + mCameraOffset, mTargetMenuPosition + mCameraOffset, movingTime);
				break;
			}

			// snap and finish movement
			if (mMovingT >= 1.0f)
			{
				transform.position = mTargetMenuPosition + mCameraOffset;
				mTargetMenuPosition = Vector3.zero;
				mStartMenuPosition = Vector3.zero;
				mMoving = false;
			}
		}
	}

	public void StartMove(GameObject menuPosition)
	{
		if (mMoving)
		{
			transform.position = mStartMenuPosition + GlobalVariables.Instance.MAIN_CAMERA_OFFSET;
		}

		mStartMenuPosition = transform.position - GlobalVariables.Instance.MAIN_CAMERA_OFFSET;
		mTargetMenuPosition = menuPosition.transform.position;
		mMoving = true;
		mMovingT = 0;
	}
	
	public PopupBuyMenu PopupBuyMenu ()
	{
		return mPopupBuyMenu;
	}

	public void HideBackButton ()
	{
		mWorldMapButton.SetActive (false);
	}

	public void HideHelpMenu ()
	{
		mHelpMenu.SetActive (false);
	}	

	public void HideOptionsMenu ()
	{
		mOptionsMenu.SetActive (false);
	}

	public void HidePopupCraftingMenu ()
	{
		mPopupCraftingMenu.SetActive (false);
	}

	public void HidePopupAchievementsMenu ()
	{
		mPopupAchievementsMenu.SetActive (false);
	}

	public void ShowBackButton ()
	{
		mWorldMapButton.SetActive (true);
	}

	public void ShowHelpMenu ()
	{
		mHelpMenu.SetActive (true);
	}	
	
	public void ShowOptionsMenu ()
	{
		mOptionsMenu.SetActive (true);
	}
	
	public void ShowPopupCraftingMenu ()
	{
		mPopupCraftingMenu.SetActive (true);
	}
	
	public void ShowPopupAchievementsMenu ()
	{
		mPopupAchievementsMenu.SetActive (true);
	}
}
