﻿using UnityEngine;
using System;
using System.Collections;

public class MainGameMenu : MonoBehaviour 
{
	// snigleton
	private static MainGameMenu instance = null;
	public static MainGameMenu Instance
	{
		get
        {
            if (Application.loadedLevelName != "MainMenuLevel")
            {
                throw new NotImplementedException();
            }
            
			if (instance == null)
            {
                instance = Singleton<MainGameMenu>.CreateInstance("Prefab/Essential/Menu/Game Menu Base");
			}
			return instance;
		}
	}

    public enum State
    {
        ERROR = -1,
        WORLD_MAP = 0,
        ITEMS = 2,
        PERKS = 1,
        CRYSTAL_SHOP = 3,

        DEFAULT = WORLD_MAP,
    };

	public GameObject mBackgroundPrefab;
	public GameObject mBackground;

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
        System.GC.Collect();
        Enable(0);
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

		MenuGUICanvas.Instance.ShowMenuButtons(show);
		mBackground.gameObject.SetActive (show);
		gameObject.SetActive (show);
	}

	public void Disable() 
	{
		ShowComponents(false);

		AudioManager.Instance.StopMusic(fmodMusic, FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
	}
	
	public void Enable(State menuState) 
	{
		AudioManager.Instance.PlayMusic(fmodMusic);

		ShowComponents(true);

        MenuCamera.Instance.transform.position = mGameMenus[(int)menuState].transform.position + GlobalVariables.Instance.MAIN_CAMERA_OFFSET;

		for (int i = 0; i < mGameMenus.Length; i++) 
		{
			mGameMenus[i].gameObject.SetActive (false);
			mGameMenus[i].Unfocus();
		}
		
		MenuGUICanvas.Instance.SetFadeColor(Color.clear);

		ResetAllMenusAndButtons ();

		mNextGameMenu = null;
        mCurrentGameMenu = mGameMenus[(int)menuState];
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
				mCurrentGameMenu.Focus();
			} 
			else if ((!focusCurrent) && (mCurrentGameMenu.IsFocused()))
			{
				mCurrentGameMenu.Unfocus();
			}
		}

		MenuCamera.Instance.ShowHelpMenu(mShowHelpMenu);
		
		MenuCamera.Instance.ShowOptionsMenu(mShowOptionsMenu);
		MenuGUICanvas.Instance.ShowOptionButtons(mShowOptionsMenu);

		MenuCamera.Instance.ShowPopupCraftingMenu(mShowPopupCraftingMenu);
		MenuGUICanvas.Instance.ShowPopupCraftingButton(mShowPopupCraftingMenu);

		MenuCamera.Instance.ShowPopupAchievementsMenu(mShowPopupAchievementsMenu);
		MenuGUICanvas.Instance.ShowPopupAchievementsButton(mShowPopupAchievementsMenu);

		bool showBack = ((mCurrentGameMenu != null) && (mGameMenus[(int)State.WORLD_MAP] != mCurrentGameMenu) && (!mMenuChangePhase));
		MenuCamera.Instance.ShowBackButton(showBack);

		MenuGUICanvas.Instance.ShowWorldMapButton(showBack && (!MenuCamera.Instance.mCotrls.activeSelf));
		MenuGUICanvas.Instance.ShowIconButtons(!MenuCamera.Instance.mCotrls.activeSelf);

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
		StartChangeGameMenu(State.WORLD_MAP);
	}
	
	public void ChangeToPerksMenu()
	{
		StartChangeGameMenu(State.PERKS);
	}
	
	public void ChangeToItemsMenu()
	{
		StartChangeGameMenu(State.ITEMS);
	}
	
	public void ChangeToChrystalShopMenu()
	{
		StartChangeGameMenu(State.CRYSTAL_SHOP);
	}

	public PerksMenu PerksMenu ()
	{
        return (PerksMenu)mGameMenus[(int)State.PERKS];
	}
	
	public ItemMenu ItemsMenu()
	{
        return (ItemMenu)mGameMenus[(int)State.ITEMS];
	}

	public WorldMapMenu WorldMapMenu ()
	{
		return (WorldMapMenu)mGameMenus[(int)State.WORLD_MAP];
	}
	
	public CrystalShopMenu CrystalShopMenu()
	{
        return (CrystalShopMenu)mGameMenus[(int)State.CRYSTAL_SHOP];
	}

	public void ResetAllMenusAndButtons ()
	{
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
			MenuGUICanvas.Instance.OptionsGUICanvas().UpdateOptions();
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

	void StartChangeGameMenu (State state)
	{
        if (mCurrentGameMenu == mGameMenus[(int)state])
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
        mNextGameMenu = mGameMenus[(int)state];

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
			//return PerksMenu().transform.Find("Perks Burst/perk_burst/Anim_BurstPerk").gameObject;
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
