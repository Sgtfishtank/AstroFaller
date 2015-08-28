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

	//public GameMenu mStartMenu;
	public GameObject mBackgroundPrefab;
	public GameObject mBackground;

	private int WORLD_MAP_MENU_INDEX = 0;
	private int ITEMS_MENU_INDEX = 2;
	private int PERKS_MENU_INDEX = 1;
	private int CRYSTAL_SHOP_MENU_INDEX = 3;

	private GameMenu[] mGameMenus;
	private GameMenu mCurrentGameMenu;
	private int mCurrentGameMenuIndex;
	private bool mShowHelpMenu = false;
	private bool mShowOptionsMenu = false;
	private bool mShowPopupCraftingMenu = false;
	private bool mShowPopupAchievementsMenu = false;

	private FMOD.Studio.EventInstance fmodMusic;
	private FMOD.Studio.EventInstance fmodSwipe;

	void Awake() 
	{
		mBackground = GameObject.Instantiate (mBackgroundPrefab);

		mGameMenus = GetComponentsInChildren<GameMenu> ();

		fmodMusic = FMOD_StudioSystem.instance.GetEvent("event:/Music/DroneMenyMusic/SpaceDrone");
		fmodSwipe = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/MenuSectionSwipe/MenuSwipeShort");
	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].Init();
		}
	}

	// Update is called once per frame
	void Update () 
	{
		if (mCurrentGameMenu == null)
		{
			if (!MenuCamera.Instance.IsMoving())
			{
				EndChangeGameMenu();
			}
		}
		else 
		{

		}
	}
	
	void ShowComponents(bool show)
	{
		MenuCamera.Instance.gameObject.SetActive (show);
		GUICanvas.Instance.ShowInGameButtons(show);
		
		GUICanvas.Instance.ShowMenuButtons(show);
		GUICanvas.Instance.ShowIconButtons(show);
		
		if (mBackground != null) 
		{
			mBackground.gameObject.SetActive (show);
		}

		gameObject.SetActive (show);
	}

	public void Disable() 
	{
		ShowComponents(false);

		AudioManager.Instance.StopMusic(fmodMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
	
	public void Enable(int menuIndex) 
	{
		AudioManager.Instance.PlayMusic(fmodMusic);
		
		ShowComponents(true);

		MenuCamera.Instance.transform.position = mGameMenus[menuIndex].transform.position + GlobalVariables.Instance.MAIN_CAMERA_OFFSET;

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].gameObject.SetActive (false);
			mGameMenus[i].Unfocus();
		}

		ResetAllMenusAndButtons ();

		mCurrentGameMenuIndex = menuIndex;
		mCurrentGameMenu = mGameMenus[mCurrentGameMenuIndex];
		mCurrentGameMenu.gameObject.SetActive (true);
		mCurrentGameMenu.Focus();

		UpdateMenusAndButtons ();
	}

	public void UpdateMenusAndButtons()
	{
		MenuCamera.Instance.ShowHelpMenu(mShowHelpMenu);
		
		MenuCamera.Instance.ShowOptionsMenu(mShowOptionsMenu);
		GUICanvas.Instance.ShowOptionButtons(mShowOptionsMenu);

		MenuCamera.Instance.ShowPopupCraftingMenu(mShowPopupCraftingMenu);
		GUICanvas.Instance.ShowPopupCraftingButton(mShowPopupCraftingMenu);

		MenuCamera.Instance.ShowPopupAchievementsMenu(mShowPopupAchievementsMenu);
		GUICanvas.Instance.ShowPopupAchievementsButton(mShowPopupAchievementsMenu);

		bool showBack = (!mGameMenus [WORLD_MAP_MENU_INDEX].IsFocused () && (mCurrentGameMenu != null));
		MenuCamera.Instance.ShowBackButton(showBack);
		GUICanvas.Instance.ShowWorldMapButton(showBack);

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].UpdateMenusAndButtons();
		}
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
		StartChangeGameMenu(CRYSTAL_SHOP_MENU_INDEX);
	}

	public PerksMenu PerksMenu ()
	{
		return (PerksMenu)mGameMenus[PERKS_MENU_INDEX];
	}
	
	public ItemMenu ItemsMenu()
	{
		return (ItemMenu)mGameMenus[ITEMS_MENU_INDEX];
	}

	public WorldMapMenu WorldMapMenu ()
	{
		return (WorldMapMenu)mGameMenus[WORLD_MAP_MENU_INDEX];
	}
	
	public CrystalShopMenu CrystalShopMenu()
	{
		return (CrystalShopMenu)mGameMenus[CRYSTAL_SHOP_MENU_INDEX];
	}

	public void ResetAllMenusAndButtons ()
	{
		MenuCamera.Instance.PopupBuyMenu().Close();

		mShowHelpMenu = false;
		mShowOptionsMenu = false;
		mShowPopupCraftingMenu = false;
		mShowPopupAchievementsMenu = false;
	}

	public void ToggleOptions ()
	{
		bool active = mShowOptionsMenu;
		ResetAllMenusAndButtons();
		mShowOptionsMenu = !active;
		UpdateMenusAndButtons ();
	}

	public void ToggleHelp ()
	{
		bool active = mShowHelpMenu;
		ResetAllMenusAndButtons();
		mShowHelpMenu = !active;
		UpdateMenusAndButtons ();
	}

	public void ToggleCraftingMenu ()
	{
		bool active = mShowPopupCraftingMenu;
		ResetAllMenusAndButtons();
		mShowPopupCraftingMenu = !active;
		UpdateMenusAndButtons ();
	}

	public void ToggleAchievementsMenu ()
	{
		bool active = mShowPopupAchievementsMenu;
		ResetAllMenusAndButtons();
		mShowPopupAchievementsMenu = !active;
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

		mCurrentGameMenuIndex = index;
		MenuCamera.Instance.StartMenuMove (mGameMenus [mCurrentGameMenuIndex].gameObject);

		mGameMenus [index].gameObject.SetActive (true);
		UpdateMenusAndButtons();

		AudioManager.Instance.PlaySound (fmodSwipe);
	}
	
	void EndChangeGameMenu ()
	{
		mCurrentGameMenu = mGameMenus [mCurrentGameMenuIndex];
		mCurrentGameMenu.Focus();

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].gameObject.SetActive (false);
		}
		mCurrentGameMenu.gameObject.SetActive (true);

		UpdateMenusAndButtons();
		
		AudioManager.Instance.StopSound(fmodSwipe, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}

	public GameObject GUIObject (string name)
	{
		switch (name) 
		{
		default:
			return null;
		}
	}
}
