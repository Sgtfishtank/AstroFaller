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
	//public Collider2D[] mButtons;

	private GameMenu[] mGameMenus;
	private MenuCamera mMenuCamera;
	private GUICanvas mCanvas;
	private PopupBuyMenu mPopupBuyMenu;
	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	private GameObject mHelpMenu;
	private GameObject mOptionsMenu;
	private GameObject mWorldMapMenu;

	// Use this for initialization
	void Start () 
	{
		mCanvas = GameObject.Find("Canvas").GetComponent<GUICanvas>();
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
		mStartMenu.Focus();
		HideAllMenus();

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
		HideAllMenus();
		ChangeGameMenu(WORLD_MAP_MENU_INDEX);
		mCanvas.HidePerkButtons();
		mCanvas.HideItemButtons();
	}
	
	public void ChangeToPerksMenu()
	{
		HideAllMenus();
		ChangeGameMenu(PERKS_MENU_INDEX);
		mCanvas.ShowPerkButtons();
		mCanvas.HideItemButtons();
	}
	
	public void ChangeToItemsMenu()
	{
		HideAllMenus();
		ChangeGameMenu(ITEMS_MENU_INDEX);
		mCanvas.HidePerkButtons();
		mCanvas.ShowItemButtons();
	}
	
	public void ChangeToChrystalShopMenu()
	{
		HideAllMenus();
		ChangeGameMenu(CRYSTAL_STORE_MENU_INDEX);
		mCanvas.HidePerkButtons();
		mCanvas.HideItemButtons();
	}

	public PerksMenu PerksMenu ()
	{
		return (PerksMenu)mGameMenus [PERKS_MENU_INDEX];
	}
	
	public ItemMenu ItemsMenu ()
	{
		return (ItemMenu)mGameMenus[ITEMS_MENU_INDEX];
	}

	public PopupBuyMenu PopupBuyMenu ()
	{
		return mPopupBuyMenu;
	}

	void HideAllMenus ()
	{
		mPopupBuyMenu.Close ();
		mHelpMenu.SetActive (false);
		mOptionsMenu.SetActive(false);
		mPopupCraftingMenu.SetActive(false);
		mPopupAchievementsMenu.SetActive(false);
		mCanvas.HidePopupCraftingButton();
		mCanvas.HidePopupAchievementsButton();
	}

	public void ToggleOptions ()
	{
		bool active = mHelpMenu.activeSelf;
		HideAllMenus();
		mHelpMenu.SetActive (!active);
	}

	public void ToggleHelp ()
	{
		bool active = mOptionsMenu.activeSelf;
		HideAllMenus();
		mOptionsMenu.SetActive(!active);
	}

	public void ToggleCraftingMenu ()
	{
		bool active = mPopupCraftingMenu.activeSelf;
		HideAllMenus();
		mPopupCraftingMenu.SetActive(!active);
		if (!active) 
		{
			mCanvas.ShowPopupCraftingButton();
		}
		else 
		{
			mCanvas.HidePopupCraftingButton();
		}
	}

	public void ToggleAchievementsMenu ()
	{
		bool active = mPopupAchievementsMenu.activeSelf;
		HideAllMenus();
		mPopupAchievementsMenu.SetActive(!active);
		if (!active)
		{
			mCanvas.ShowPopupAchievementsButton();
		}
		else 
		{
			mCanvas.HidePopupAchievementsButton();
		}
	}
	
	public void BuyWithBolts()
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			if (mGameMenus[i].IsFocused())
			{
				mGameMenus[i].BuyWithBolts();
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
			}
		}
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
		mCanvas.HideBackButton ();
		mWorldMapMenu.SetActive (false);
	}
	
	void ShowBackButton ()
	{
		mCanvas.ShowBackButton ();
		mWorldMapMenu.SetActive (true);
	}
}
