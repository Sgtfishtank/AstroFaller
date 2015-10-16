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

	public GameObject mBackgroundPrefab;
	public GameObject mBackground;

	private int WORLD_MAP_MENU_INDEX = 0;
	private int PERKS_MENU_INDEX = 1;
	private int ITEMS_MENU_INDEX = 2;
	private int CRYSTAL_SHOP_MENU_INDEX = 3;

	private GameMenu[] mGameMenus;
	public GameMenu mCurrentGameMenu;
	private GameMenu mNextGameMenu;

	private bool mShowHelpMenu = false;
	private bool mShowPopupCraftingMenu = false;
	private bool mShowPopupAchievementsMenu = false;

	private bool mMenuChangePhase;
	
	private bool mShowOptionsMenu = false;
	private AudioManager mSettingAudioManagerBackup;

	private FMOD.Studio.EventInstance fmodMusic;
	private FMOD.Studio.EventInstance fmodSwipe;

	void Awake() 
	{
		mSettingAudioManagerBackup = GetComponent<AudioManager> ();

		if (mBackgroundPrefab != null) 
		{
			mBackground = GameObject.Instantiate (mBackgroundPrefab);
		}
		else
		{
			mBackground = new GameObject("Menu Background");
		}

		mGameMenus = GetComponentsInChildren<GameMenu> ();

		fmodMusic = FMOD_StudioSystem.instance.GetEvent("event:/Music/DroneMenyMusic/SpaceDrone");
		fmodSwipe = FMOD_StudioSystem.instance.GetEvent("event:/Sounds/MenuSectionSwipe/MenuSwipeShort");

		mCurrentGameMenu = mGameMenus [0];
	}

	// Use this for initialization
	void Start () 
	{
		
		MenuCamera.Instance.mCotrls.SetActive(true);
	}

	// Update is called once per frame
	void Update () 
	{
		if (mMenuChangePhase)
		{
			if (!MenuCamera.Instance.IsMoving())
			{
				EndChangeGameMenu();
			}
		}
	}
	
	void ShowComponents(bool show)
	{
		MenuCamera.Instance.gameObject.SetActive (show);

		GUICanvas.Instance.ShowMenuButtons(show);
		GUICanvas.Instance.MenuGUICanvas().ShowIconButtons(show);
		mBackground.gameObject.SetActive (show);
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
		
		GUICanvas.Instance.MenuGUICanvas().SetFadeColor(Color.clear);

		ResetAllMenusAndButtons ();

		mNextGameMenu = null;
		mCurrentGameMenu = mGameMenus[menuIndex];
		mCurrentGameMenu.gameObject.SetActive (true);
		mCurrentGameMenu.Focus();

		UpdateMenusAndButtons ();
	}

	public void UpdateMenusAndButtons()
	{
		if (!mMenuChangePhase) 
		{
			bool focusCurrent = !(mShowOptionsMenu || mShowHelpMenu || mShowPopupAchievementsMenu || mShowPopupCraftingMenu);
			if (focusCurrent && (!mCurrentGameMenu.IsFocused())) 
			{
				mCurrentGameMenu.Focus ();
			} 
			else if ((!focusCurrent) && (mCurrentGameMenu.IsFocused()))
			{
				mCurrentGameMenu.Unfocus ();
			}
		}

		MenuCamera.Instance.ShowHelpMenu(mShowHelpMenu);
		
		MenuCamera.Instance.ShowOptionsMenu(mShowOptionsMenu);
		GUICanvas.Instance.ShowOptionButtons(mShowOptionsMenu);

		MenuCamera.Instance.ShowPopupCraftingMenu(mShowPopupCraftingMenu);
		GUICanvas.Instance.MenuGUICanvas().ShowPopupCraftingButton(mShowPopupCraftingMenu);

		MenuCamera.Instance.ShowPopupAchievementsMenu(mShowPopupAchievementsMenu);
		GUICanvas.Instance.MenuGUICanvas().ShowPopupAchievementsButton(mShowPopupAchievementsMenu);

		bool showBack = ((mCurrentGameMenu != null) && (mGameMenus[WORLD_MAP_MENU_INDEX] != mCurrentGameMenu) && (!mMenuChangePhase));
		MenuCamera.Instance.ShowBackButton(showBack);
		GUICanvas.Instance.MenuGUICanvas().ShowWorldMapButton(showBack);

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].UpdateMenusAndButtons();
		}
	}
	public int CurrentMenu()
	{
		for (int i = 0; i < mGameMenus.Length; i++)
		{
			if(mCurrentGameMenu == mGameMenus[i])
			{
				return i;
			}
		}
		return -1;
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
		MenuCamera.Instance.mCotrls.SetActive(false);
		if (MenuCamera.Instance.PopupBuyMenu().IsOpen()) 
		{
			MenuCamera.Instance.PopupBuyMenu().Close ();
		}

		mShowHelpMenu = false;
		mShowOptionsMenu = false;
		mShowPopupCraftingMenu = false;
		mShowPopupAchievementsMenu = false;
	}

	public void ToggleOptions ()
	{
		ShowOptions (!mShowOptionsMenu, false);
	}

	public void ShowOptions(bool show, bool cancelChanges)
	{
		ResetAllMenusAndButtons();

		mShowOptionsMenu = show;

		// if show -> backup current state 
		// if not show -> if apply -> revert current state to backup state
		if (mShowOptionsMenu) 
		{
			// backup audio current state on open options
			AudioManager.Instance.CopyState (mSettingAudioManagerBackup);
		}
		else if ((!mShowOptionsMenu) && cancelChanges) 
		{
			// apply backup state on close options if cancel changes made in options
			mSettingAudioManagerBackup.CopyState(AudioManager.Instance);
			GUICanvas.Instance.OptionsGUICanvas().UpdateOptions();
		}

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
		
		UpdateMenusAndButtons ();
	}
	
	public void BuyWithCrystals()
	{
		if (mCurrentGameMenu == null)
		{
			print("ERROR nothing focus in BuyWithCrystals");
			return;
		}

		mCurrentGameMenu.BuyWithCrystals();
		
		UpdateMenusAndButtons ();
	}

	void StartChangeGameMenu (int index)
	{
		if (mCurrentGameMenu == mGameMenus[index])
		{
			if (mMenuChangePhase)
			{
				// switch
				GameMenu a = mNextGameMenu;
				mNextGameMenu = mCurrentGameMenu;
				mCurrentGameMenu = a;
				MenuCamera.Instance.StartMenuMove (mNextGameMenu.gameObject);
				return;
			}
			else
			{
				return; // don't move if already there
			}
		}

		ResetAllMenusAndButtons ();

		mMenuChangePhase = true;
		mNextGameMenu = mGameMenus[index];

		mCurrentGameMenu.Unfocus();
		mNextGameMenu.gameObject.SetActive (true);

		MenuCamera.Instance.StartMenuMove (mNextGameMenu.gameObject);
		AudioManager.Instance.PlaySound (fmodSwipe);

		UpdateMenusAndButtons();
	}
	
	void EndChangeGameMenu ()
	{
		mMenuChangePhase = false;
	
		mCurrentGameMenu.gameObject.SetActive (false);
		mNextGameMenu.Focus();
		
		mCurrentGameMenu = mNextGameMenu;
		mNextGameMenu = null;

		AudioManager.Instance.StopSound(fmodSwipe, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
		
		UpdateMenusAndButtons();
	}

	public GameObject GUIObject (string name)
	{
		switch (name) 
		{
		case "Button 7":
			return PerksMenu().transform.Find("Perks Burst/perk_burst/Anim_BurstPerk").gameObject;
		case "Button 1":
			//return PerksMenu().transform.Find("Perks Air/perk_air/Anim_AirPerk").gameObject;
		case "Button 4":
			//return PerksMenu().transform.Find("Perks Life/perk_life/Anim_LifePerk").gameObject;
		case "RocketThrust":
			return ItemsMenu().transform.Find("Rocket Thrust/item_megaburst/item_megaburst").gameObject;
		case "UnlimitedAir":
			return ItemsMenu().transform.Find("Unlimited Air/item_unlimitedair/item_unlimitedair").gameObject;
		case "Shockwave":
			return ItemsMenu().transform.Find("Shockwave/item_shockwave/item_shockwave").gameObject;
		case "ForceField":
			return ItemsMenu().transform.Find("Force Field/item_shield/item_shield").gameObject;
		case "BoltsMagnet":
			return ItemsMenu().transform.Find("Bolt Magnet/item_boltmagnet/item_boltmagnet").gameObject;
		case "BoltsMultiplier":
			return ItemsMenu().transform.Find("Bolt Multiplier/item_boltmultiplier/item_boltmultiplier").gameObject;
		default:
			return null;
		}
	}
}
