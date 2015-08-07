using UnityEngine;
using System.Collections;

public class MainGameMenu : MonoBehaviour 
{
	// snigleton
	private static MainGameMenu instance = null;
	public static MainGameMenu Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("Game Menu Base");
				instance = thisObject.GetComponent<MainGameMenu>();
			}
			return instance;
		}
	}

	private int WORLD_MAP_MENU_INDEX = 0;
	private int ITEMS_MENU_INDEX = 1;
	private int PERKS_MENU_INDEX = 2;
	private int CRYSTAL_STORE_MENU_INDEX = 3;

	public GameMenu mStartMenu;
	//public Collider2D[] mButtons;

	private GameMenu[] mGameMenus;
	private MenuCamera mMenuCamera;
	private PopupBuyMenu mPopupBuyMenu;
	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	private GameObject mHelpMenu;
	private GameObject mOptionsMenu;
	private GameObject mWorldMapMenu;

	// Use this for initialization
	void Start () 
	{
		mPopupBuyMenu = GameObject.Find("Menu Camera/PopupBuyMenu").GetComponent<PopupBuyMenu>();
		mPopupBuyMenu.Init ();

		mPopupCraftingMenu = GameObject.Find("Menu Camera/PopupCraftingMenu");
		mPopupAchievementsMenu = GameObject.Find("Menu Camera/PopupAchievementsMenu");
		mHelpMenu = GameObject.Find("Menu Camera/Help");
		mOptionsMenu = GameObject.Find("Menu Camera/Options");
		mWorldMapMenu = GameObject.Find("Menu Camera/WorldMapButton");

		mMenuCamera = GameObject.Find ("Menu Camera").GetComponent<MenuCamera>();

		mGameMenus = GetComponentsInChildren<GameMenu> ();
		if (mStartMenu == null)
		{
			mStartMenu = mGameMenus[WORLD_MAP_MENU_INDEX];
		}

		mMenuCamera.transform.position = mStartMenu.transform.position + mMenuCamera.mCameraOffset;

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Init();
		}
		
		mWorldMapMenu.SetActive (false);
		mStartMenu.Focus ();
		HideAllMenus ();

		if (mGameMenus[WORLD_MAP_MENU_INDEX].IsFocused())
		{
			HideBackButton();
		}
		else
		{
			ShowBackButton();
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (Input.GetMouseButtonUp(0))
		{
			Ray ray = mMenuCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			int mask = Physics2D.DefaultRaycastLayers;

			RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, mask);
			for (int i = 0; i < hits.Length; i++) 
			{
				print(hits[i].collider.name);
			}

			RaycastHit hit;
			if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, mask))
			{
				PressButton(hit.collider.name);
			}
			else
			{
				HideAllMenus();
			}
		}

		if (Input.touchCount > 0) 
		{

		}
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			ChangeGameMenu(0);
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			ChangeGameMenu(1);
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			ChangeGameMenu(2);
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			ChangeGameMenu(3);
		}
	}

	public PopupBuyMenu PopupBuyMenu ()
	{
		return mPopupBuyMenu;
	}

	void PressButton (string buttonName)
	{
		switch (buttonName) 
		{
		case "WorldMapButton":
			ChangeGameMenu(WORLD_MAP_MENU_INDEX);
			break;
		case "CraftingButton":
			ToggleCraftingMenu();
			break;
		case "AchievementsButton":
			ToggleAchievementsMenu();
			break;
		case "OptionsButton":
			ToggleOptions();
			break;
		case "HelpButton":
			ToggleHelp();
			break;
		case "QuestsButton":
			HideAllMenus();
			break;
		case "StatsButton":
			HideAllMenus();
			break;
		case "KimJongUnBoardsButton":
			HideAllMenus();
			break;
		case "ItemsButton":
			ChangeGameMenu(ITEMS_MENU_INDEX);
			HideAllMenus();
			break;
		case "PerksButton":
			ChangeGameMenu(PERKS_MENU_INDEX);
			HideAllMenus();
			break;
		case "CrystalStoreButton":
			ChangeGameMenu(CRYSTAL_STORE_MENU_INDEX);
			HideAllMenus();
			break;
		case "BackButton":
			ChangeGameMenu(WORLD_MAP_MENU_INDEX);
			HideAllMenus();
			break;
		default:
			print("Error button " + buttonName);
			break;
		}
	}

	void HideAllMenus ()
	{
		mPopupBuyMenu.Close ();
		mHelpMenu.SetActive (false);
		mOptionsMenu.SetActive(false);
		mPopupCraftingMenu.SetActive(false);
		mPopupAchievementsMenu.SetActive(false);
	}

	void ToggleOptions ()
	{
		bool active = mHelpMenu.activeSelf;
		HideAllMenus();
		mHelpMenu.SetActive (!active);
	}

	void ToggleHelp ()
	{
		bool active = mOptionsMenu.activeSelf;
		HideAllMenus();
		mOptionsMenu.SetActive(!active);
	}

	void ToggleCraftingMenu ()
	{
		bool active = mPopupCraftingMenu.activeSelf;
		HideAllMenus();
		mPopupCraftingMenu.SetActive(!active);
	}

	void ToggleAchievementsMenu ()
	{
		bool active = mPopupAchievementsMenu.activeSelf;
		HideAllMenus();
		mPopupAchievementsMenu.SetActive(!active);
	}

	void ChangeGameMenu (int index)
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Unfocus();
		}

		mMenuCamera.StartMove (mGameMenus [index].gameObject);
		mGameMenus[index].Focus();
		
		if (mGameMenus[WORLD_MAP_MENU_INDEX].IsFocused())
		{
			HideBackButton();
		}
		else
		{
			ShowBackButton();
		}
	}
	
	void HideBackButton ()
	{
		mWorldMapMenu.SetActive (false);
	}
	
	void ShowBackButton ()
	{
		mWorldMapMenu.SetActive (true);
	}
}
