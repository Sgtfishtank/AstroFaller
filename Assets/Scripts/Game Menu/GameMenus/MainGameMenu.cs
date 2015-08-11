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

		MenuCamera.Instance.transform.position = mStartMenu.transform.position + MenuCamera.Instance.mCameraOffset;

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Init();
		}

		mStartMenu.Focus();
	}

	// Update is called once per frame
	void Update () 
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
				//HideAllMenus();
			}
		}
		
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			ChangeToWorldMapMenu();
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			ChangeToItemsMenu();
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			ChangeToPerksMenu();
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			ChangeToChrystalShopMenu();
		}
	}

	public void ChangeToWorldMapMenu()
	{
		ResetAllMenus();
		ChangeGameMenu(WORLD_MAP_MENU_INDEX);
		GUICanvas.Instance.HideBackButton ();
		MenuCamera.Instance.HideBackButton ();
		GUICanvas.Instance.HidePerkButtons();
		GUICanvas.Instance.HideItemButtons();
	}
	
	public void ChangeToPerksMenu()
	{
		ResetAllMenus();
		ChangeGameMenu(PERKS_MENU_INDEX);
		GUICanvas.Instance.ShowBackButton ();
		MenuCamera.Instance.ShowBackButton ();
		GUICanvas.Instance.ShowPerkButtons();
		GUICanvas.Instance.HideItemButtons();
	}
	
	public void ChangeToItemsMenu()
	{
		ResetAllMenus();
		ChangeGameMenu(ITEMS_MENU_INDEX);
		GUICanvas.Instance.ShowBackButton ();
		MenuCamera.Instance.ShowBackButton ();
		GUICanvas.Instance.HidePerkButtons();
		GUICanvas.Instance.ShowItemButtons();
	}
	
	public void ChangeToChrystalShopMenu()
	{
		ResetAllMenus();
		ChangeGameMenu(CRYSTAL_STORE_MENU_INDEX);
		GUICanvas.Instance.ShowBackButton ();
		MenuCamera.Instance.ShowBackButton ();
		GUICanvas.Instance.HidePerkButtons();
		GUICanvas.Instance.HideItemButtons();
	}

	public PerksMenu PerksMenu ()
	{
		return (PerksMenu)mGameMenus [PERKS_MENU_INDEX];
	}
	
	public ItemMenu ItemsMenu ()
	{
		return (ItemMenu)mGameMenus[ITEMS_MENU_INDEX];
	}

	public void ResetAllMenus ()
	{
		mShowHelpMenu = false;
		mShowOptionsMenu = false;
		mShowPopupCraftingMenu = false;
		mShowPopupAchievementsMenu = false;

		MenuCamera.Instance.PopupBuyMenu().Close();
		MenuCamera.Instance.HideHelpMenu();
		MenuCamera.Instance.HideOptionsMenu();
		MenuCamera.Instance.HidePopupCraftingMenu();
		GUICanvas.Instance.HidePopupCraftingButton();
		MenuCamera.Instance.HidePopupAchievementsMenu();
		GUICanvas.Instance.HidePopupAchievementsButton();
	}

	public void ToggleOptions ()
	{
		bool active = mShowOptionsMenu;
		ResetAllMenus();
		mShowOptionsMenu = !active;
		if (mShowOptionsMenu) 
		{
			MenuCamera.Instance.ShowOptionsMenu();
		}
		else 
		{
			MenuCamera.Instance.HideOptionsMenu();
		}
	}

	public void ToggleHelp ()
	{
		bool active = mShowHelpMenu;
		ResetAllMenus();
		mShowHelpMenu = !active;
		if (mShowHelpMenu) 
		{
			MenuCamera.Instance.ShowHelpMenu();
		}
		else 
		{
			MenuCamera.Instance.HideOptionsMenu();
		}
	}

	public void ToggleCraftingMenu ()
	{
		bool active = mShowPopupCraftingMenu;
		ResetAllMenus();
		mShowPopupCraftingMenu = !active;
		if (mShowPopupCraftingMenu) 
		{
			MenuCamera.Instance.ShowPopupCraftingMenu();
			GUICanvas.Instance.ShowPopupCraftingButton();
		}
		else 
		{
			MenuCamera.Instance.HidePopupCraftingMenu();
			GUICanvas.Instance.HidePopupCraftingButton();
		}
	}

	public void ToggleAchievementsMenu ()
	{
		bool active = mShowPopupAchievementsMenu;
		ResetAllMenus();
		mShowPopupAchievementsMenu = !active;
		if (mShowPopupAchievementsMenu)
		{
			MenuCamera.Instance.ShowPopupAchievementsMenu();
			GUICanvas.Instance.ShowPopupAchievementsButton();
		}
		else 
		{
			MenuCamera.Instance.HidePopupAchievementsMenu();
			GUICanvas.Instance.HidePopupAchievementsButton();
		}
	}
	
	public void BuyWithBolts()
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			if (mGameMenus[i].IsFocused())
			{
				mGameMenus[i].BuyWithBolts();
				return;
			}
		}
	}
	
	public void BuyWithCrystals()
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			if (mGameMenus[i].IsFocused())
			{
				mGameMenus[i].BuyWithCrystals();
				return;
			}
		}
	}

	void ChangeGameMenu (int index)
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Unfocus();
		}

		MenuCamera.Instance.StartMove (mGameMenus [index].gameObject);
		mGameMenus[index].Focus();
	}	

	public void ShowBuyButtons ()
	{
		if (mGameMenus[PERKS_MENU_INDEX].IsFocused())
		{
			GUICanvas.Instance.HideItemButtons();
			GUICanvas.Instance.ShowPerkButtons();
		}
		else if (mGameMenus[ITEMS_MENU_INDEX].IsFocused())
		{
			GUICanvas.Instance.HidePerkButtons();
			GUICanvas.Instance.ShowItemButtons();
		}
	}

	public void HideBuyButtons ()
	{
		GUICanvas.Instance.HidePerkButtons();
		GUICanvas.Instance.HideItemButtons();
	}
}
