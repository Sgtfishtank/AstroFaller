using UnityEngine;
using System;
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
				if (Application.loadedLevelName != "MainMenuLevel")
				{
					throw new NotImplementedException();
				}

                instance = Singleton<MenuCamera>.CreateInstance("Prefab/Essential/Menu/Menu Camera");
			}
			return instance;
		}
	}

	public enum MoveType
	{
		Linerar,
		LinerarSaw,
		IsoscelesTriangle,
		Hyperbole,
	}

	public MoveType mDefaultCameraMoveType;
	public bool mDefaultUseSmoothStep;
	public GameObject mPopupCraftingMenuPrefab;
	public GameObject mPopupAchievementsMenuPrefab;
	public GameObject mHelpMenuPrefab;
	public GameObject mOptionsMenuPrefab;
	public GameObject mPlayButtonPrefab;

	private MoveType mCameraMoveType;
	private bool mUseSmoothStep;

	private Vector3 mStartMenuPosition;
	private Vector3 mTargetMenuPosition;
	private bool mMoving;
	private float mMovingT;

	private PopupBuyMenu mPopupBuyMenu;

	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	public GameObject[] mHelpMenu = new GameObject[3];
	private GameObject mOptionsMenu;
	private GameObject mWorldMapButton;
	private GameObject mWorldMapIcon;
	private TextMesh mBoltsText;
	private Camera mCamera;
	private int mBolts = -1;
	
	public GameObject mCotrls;
	
	//private ButtonManager mWorkshopIcon; 
	//private ButtonManager msettingsIcon; 
	//private ButtonManager mOptionsIcon; 
	//private ButtonManager mAchivIcon; 

	void Awake()
	{
		mCamera = GetComponent<Camera> ();
		mCotrls = transform.Find("player_controls 1").gameObject;
		mBoltsText = transform.Find("Bolts/Total_Bolts_Text").GetComponent<TextMesh>();

		//mHelpMenu[0] = transform.Find("question_menu_worldmap").gameObject;
		//mHelpMenu[1] = transform.Find("question_menu_Items").gameObject;
		//mHelpMenu[2] = transform.Find("question_menu_Items").gameObject;
		mOptionsMenu = transform.Find("Options").gameObject;
		mWorldMapButton = transform.Find("Icons/worldmap_icon").gameObject;
		mPopupCraftingMenu = transform.Find("PopupCraftingMenu").gameObject;
		mPopupAchievementsMenu = transform.Find("PopupAchievementsMenu").gameObject;
		mPopupBuyMenu = transform.Find("PopupBuyMenu").GetComponent<PopupBuyMenu>();

		GlobalVariables.Instance.Instanciate (mPopupCraftingMenuPrefab, mPopupCraftingMenu.transform, 19);
		GlobalVariables.Instance.Instanciate (mPopupAchievementsMenuPrefab, mPopupAchievementsMenu.transform, 19);
		//GlobalVariables.Instance.Instanciate (mHelpMenuPrefab, mHelpMenu.transform, 10);
		GlobalVariables.Instance.Instanciate (mOptionsMenuPrefab, mOptionsMenu.transform, 15);
		mHelpMenu [0].SetActive (false);
		mHelpMenu [1].SetActive (false);
		mHelpMenu [2].SetActive (false);
	}

	// Use this for initialization
	void Start () 
	{
		// ugly gui setup
		GUICanvasBase gui = MenuGUICanvas.Instance.IconsGUI();
		ButtonManager.CreateButton(gameObject, "Icons/workshop_icon", "CraftingButton", gui);	
		ButtonManager.CreateButton(gameObject, "Icons/settings_icon", "OptionsButton", gui);	
		ButtonManager.CreateButton(gameObject, "Icons/info_icon", "HelpButton", gui);	
		ButtonManager.CreateButton(gameObject, "Icons/achievement_icon", "AchievementsButton", gui);
		ButtonManager.CreateButton(gameObject, "Icons/worldmap_icon", "WorldMapButton", gui);
		
		gui = MenuGUICanvas.Instance.AchievementsMenu();
		ButtonManager.CreateButton(gameObject, "PopupAchievementsMenu/pop_up_achievementsmenu_new/Button_1", "QuestsButton", gui);
		ButtonManager.CreateButton(gameObject, "PopupAchievementsMenu/pop_up_achievementsmenu_new/Button_2", "StatsButton", gui);
		ButtonManager.CreateButton(gameObject, "PopupAchievementsMenu/pop_up_achievementsmenu_new/Button_3", "KimJongUnBoardsButton", gui);
		
		gui = MenuGUICanvas.Instance.CraftingMenu();
		ButtonManager.CreateButton(gameObject, "PopupCraftingMenu/pop_up_craftingmenu_new/Button_1", "ItemsButton", gui);
		ButtonManager.CreateButton(gameObject, "PopupCraftingMenu/pop_up_craftingmenu_new/Button_2", "PerksButton", gui);
		ButtonManager.CreateButton(gameObject, "PopupCraftingMenu/pop_up_craftingmenu_new/Button_3", "CrystalStoreButton", gui);
		
		gui = MenuGUICanvas.Instance.OptionsGUICanvas();
		ButtonManager.CreateButton(gameObject, "Options/settings_pop_up/Button_1", "OptionButtons/SettingsYes", gui);
		ButtonManager.CreateButton(gameObject, "Options/settings_pop_up/Button_2", "OptionButtons/SettingsNo", gui);

        mCotrls.SetActive(PlayerData.Instance.mShowControls);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 mCameraOffset = GlobalVariables.Instance.MAIN_CAMERA_OFFSET;
		if (mMoving) 
		{
			mMovingT = Mathf.Clamp01(mMovingT + (Time.deltaTime * GlobalVariables.Instance.MAIN_CAMERA_OFFSET_MOVE_SPEED));

			float movingTime =  MovingT();

			float dist = Vector3.Distance(mStartMenuPosition, mTargetMenuPosition);
			Vector3 midPoint = ((mStartMenuPosition + mTargetMenuPosition) * 0.5f) + new Vector3(0, 0, dist) * GlobalVariables.Instance.MAIN_CAMERA_MOVE_ZOOM_OUT_FACTOR;
			switch (mCameraMoveType) 
			{
			case MoveType.LinerarSaw:
				transform.position = Vector3.Lerp(mStartMenuPosition + mCameraOffset, mTargetMenuPosition + mCameraOffset, movingTime * movingTime * movingTime);
				break;
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
					float x = Mathf.Lerp(mStartMenuPosition.x, midPoint.x, movingTimePart);
					float y = Mathf.Lerp(mStartMenuPosition.y, midPoint.y, movingTimePart);
					float z = mStartMenuPosition.z + zdiff - (zdiff * Mathf.Pow(1 - movingTimePart, 2));

					// prevent other dimensions from appearing
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
					float x = Mathf.Lerp(midPoint.x, mTargetMenuPosition.x, movingTimePart);
					float y = Mathf.Lerp(midPoint.y, mTargetMenuPosition.y, movingTimePart);
					float z = midPoint.z - (zdiff * Mathf.Pow(movingTimePart, 2));
					
					// prevent other dimensions from appearing
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
				mMoving = false;
			}
		}

		// avoid string allocations
		if (mBolts != PlayerData.Instance.bolts()) 
		{
			mBolts = PlayerData.Instance.bolts();
			mBoltsText.text = mBolts.ToString();
		}
	}

	public bool IsMoving ()
	{
		return mMoving;
	}

	public float MovingT ()
	{
		if (mUseSmoothStep)
		{
			return Mathf.SmoothStep(0, 1, mMovingT);
		}

		return mMovingT;
	}
	
	public Camera Camera()
	{
		return mCamera;
	}

	public void StartMenuMove(GameObject menuPosition)
	{
		mTargetMenuPosition = menuPosition.transform.position;
		mCameraMoveType = mDefaultCameraMoveType;
		mUseSmoothStep = mDefaultUseSmoothStep;

		StartCameraMove ();
	}

	public void StartLevelZoom ()
	{
		mTargetMenuPosition = transform.position - GlobalVariables.Instance.MAIN_CAMERA_OFFSET + GlobalVariables.Instance.MAIN_CAMERA_START_LEVEL_ZOOM;
		mCameraMoveType = MoveType.LinerarSaw;
		mUseSmoothStep = false;

		StartCameraMove ();
	}

	void StartCameraMove()
	{	
		if (mMoving)
		{
			transform.position = mStartMenuPosition + GlobalVariables.Instance.MAIN_CAMERA_OFFSET;
		}

		mStartMenuPosition = transform.position - GlobalVariables.Instance.MAIN_CAMERA_OFFSET;

		mMoving = true;
		mMovingT = 0;

		if (Vector3.Distance(mStartMenuPosition, mTargetMenuPosition) < 0.01f)
		{
			mMovingT = 1;
		}
	}

	public PopupBuyMenu PopupBuyMenu ()
	{
		return mPopupBuyMenu;
	}

	public void ShowBackButton (bool show)
	{
		mWorldMapButton.SetActive (show);
	}

	public void ShowHelpMenu (bool show)
	{
		switch (MainGameMenu.Instance.CurrentMenu())
		{
		case 0:
			mHelpMenu[0].SetActive(show);
			break;
		case 1:
			mHelpMenu[1].SetActive(show);
			break;
		case 2:
			mHelpMenu[2].SetActive(show);
			break;
		default:
			break;
		}
		//mHelpMenu.SetActive (show);
	}
	
	public void ShowOptionsMenu (bool show)
	{
		mOptionsMenu.SetActive (show);
	}
	
	public void ShowPopupCraftingMenu (bool show)
	{
		mPopupCraftingMenu.SetActive (show);
	}
	
	public void ShowPopupAchievementsMenu (bool show)
	{
		mPopupAchievementsMenu.SetActive (show);
	}
}
