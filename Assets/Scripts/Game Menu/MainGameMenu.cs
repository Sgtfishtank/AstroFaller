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
	private int ITEMS_MENU_INDEX = 2;
	private int PERKS_MENU_INDEX = 1;
	private int CRYSTAL_STORE_MENU_INDEX = 3;

	public GameMenu mStartMenu;

	private GameMenu[] mGameMenus;
	private GameMenu mCurrentGameMenu;
	private int mCurrentGameMenuIndex;
	private bool mShowHelpMenu = false;
	private bool mShowOptionsMenu = false;
	private bool mShowPopupCraftingMenu = false;
	private bool mShowPopupAchievementsMenu = false;

	// Use this for initialization
	void Start () 
	{
		mGameMenus = GetComponentsInChildren<GameMenu> ();
		if (mStartMenu == null)
		{
			mStartMenu = mGameMenus[WORLD_MAP_MENU_INDEX];
		}

		MenuCamera.Instance.transform.position = mStartMenu.transform.position + GlobalVariables.Instance.MAIN_CAMERA_OFFSET;

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Init();
		}

		mStartMenu.Focus();
	}

	// Update is called once per frame
	void Update () 
	{
		if (mCurrentGameMenu == null)
		{
			if (!MenuCamera.Instance.IsMoving())
			{
				EndChangeGameMenu();

				UpdateMenusAndButtons();
			}
		}
		else 
		{
			if (Input.GetMouseButtonUp(0))
			{
				Ray ray = MenuCamera.Instance.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
				int mask = Physics2D.DefaultRaycastLayers;

				RaycastHit[] hits = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity, mask);
				for (int i = 0; i < hits.Length; i++) 
				{
					//print(hits[i].collider.name);
				}

				RaycastHit hit;
				if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity, mask))
				{
					//PressButton(hit.collider.name);
				}
				else
				{
					//ShowAllMenus();
				}
			}
		}
	}

	public void UpdateMenusAndButtons()
	{
		ShowBackButton(!mGameMenus[WORLD_MAP_MENU_INDEX].IsFocused() && (mCurrentGameMenu != null));
		GUICanvas.Instance.ShowPerkButtons(mGameMenus[PERKS_MENU_INDEX].IsFocused() && !MenuCamera.Instance.PopupBuyMenu().IsOpen());
		GUICanvas.Instance.ShowItemButtons(mGameMenus[ITEMS_MENU_INDEX].IsFocused() && !MenuCamera.Instance.PopupBuyMenu().IsOpen());
	}

	public void ChangeToWorldMapMenu()
	{
		StartChangeGameMenu(WORLD_MAP_MENU_INDEX);
	}
	
	public void ChangeToPerksMenu()
	{
		StartChangeGameMenu(PERKS_MENU_INDEX);
	}
	
	public void ChangeToItemsMenu()
	{
		StartChangeGameMenu(ITEMS_MENU_INDEX);
	}
	
	public void ChangeToChrystalShopMenu()
	{
		StartChangeGameMenu(CRYSTAL_STORE_MENU_INDEX);
	}

	public PerksMenu PerksMenu ()
	{
		return (PerksMenu)mGameMenus [PERKS_MENU_INDEX];
	}
	
	public ItemMenu ItemsMenu ()
	{
		return (ItemMenu)mGameMenus[ITEMS_MENU_INDEX];
	}

	public void ResetAllMenusAndButtons ()
	{
		MenuCamera.Instance.PopupBuyMenu().Close();

		ShowHelpMenu(false);
		ShowOptionsMenu(false);
		ShowPopupCraftingMenu(false);
		ShowPopupAchievementsMenu(false);
	}
	
	void ShowHelpMenu(bool show)
	{
		mShowHelpMenu = show;
		MenuCamera.Instance.ShowHelpMenu(show);
	}
	
	void ShowOptionsMenu(bool show)
	{
		mShowOptionsMenu = show;
		MenuCamera.Instance.ShowOptionsMenu(show);
	}

	void ShowPopupCraftingMenu(bool show)
	{
		mShowPopupCraftingMenu = show;
		MenuCamera.Instance.ShowPopupCraftingMenu(show);
		GUICanvas.Instance.ShowPopupCraftingButton(show);
	}	

	void ShowPopupAchievementsMenu(bool show)
	{
		mShowPopupAchievementsMenu = show;
		MenuCamera.Instance.ShowPopupAchievementsMenu(show);
		GUICanvas.Instance.ShowPopupAchievementsButton(show);
	}
	
	void ShowBackButton(bool show)
	{
		GUICanvas.Instance.ShowBackButton(show);
		MenuCamera.Instance.ShowBackButton(show);
	}

	public void ToggleOptions ()
	{
		bool active = mShowOptionsMenu;
		ResetAllMenusAndButtons();
		mShowOptionsMenu = !active;
		MenuCamera.Instance.ShowOptionsMenu(mShowOptionsMenu);
		UpdateMenusAndButtons ();
	}

	public void ToggleHelp ()
	{
		bool active = mShowHelpMenu;
		ResetAllMenusAndButtons();
		mShowHelpMenu = !active;
		MenuCamera.Instance.ShowHelpMenu(mShowHelpMenu);
		UpdateMenusAndButtons ();
	}

	public void ToggleCraftingMenu ()
	{
		bool active = mShowPopupCraftingMenu;
		ResetAllMenusAndButtons();
		mShowPopupCraftingMenu = !active;
		MenuCamera.Instance.ShowPopupCraftingMenu(mShowPopupCraftingMenu);
		GUICanvas.Instance.ShowPopupCraftingButton(mShowPopupCraftingMenu);
		UpdateMenusAndButtons ();
	}

	public void ToggleAchievementsMenu ()
	{
		bool active = mShowPopupAchievementsMenu;
		ResetAllMenusAndButtons();
		mShowPopupAchievementsMenu = !active;
		MenuCamera.Instance.ShowPopupAchievementsMenu(mShowPopupAchievementsMenu);
		GUICanvas.Instance.ShowPopupAchievementsButton(mShowPopupAchievementsMenu);
		UpdateMenusAndButtons ();
	}
	
	public void BuyWithBolts()
	{
		if (mCurrentGameMenu == null)
		{
			print("ERROR nothing focus in BuyWithBolts");
			return;
		}

		mCurrentGameMenu.BuyWithBolts();
	}
	
	public void BuyWithCrystals()
	{
		if (mCurrentGameMenu == null)
		{
			print("ERROR nothing focus in BuyWithCrystals");
			return;
		}

		mCurrentGameMenu.BuyWithCrystals();
	}

	void StartChangeGameMenu (int index)
	{
		ResetAllMenusAndButtons ();

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Unfocus();
		}
		mCurrentGameMenu = null;

		MenuCamera.Instance.StartMove (mGameMenus [index].gameObject);
		mCurrentGameMenuIndex = index;
		
		UpdateMenusAndButtons();
	}
	
	void EndChangeGameMenu ()
	{
		mCurrentGameMenu = mGameMenus [mCurrentGameMenuIndex];
		mCurrentGameMenu.Focus();

		UpdateMenusAndButtons();
	}
}
