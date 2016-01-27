using UnityEngine;
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
			if (instance == null)
            {
                if (PlayerData.Instance.CurrentScene() != PlayerData.Scene.MAIN_MENU)
                {
                    throw new NotImplementedException();
                }

                Singleton<MainGameMenu>.CreateInstance("Prefab/Game Menu/Game Menu Base");
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

	private AudioInstanceData fmodMusic;
	private AudioInstanceData fmodSwipe;

	void Awake() 
	{
		if (instance == null)
		{
			instance = this;
		}

		mSettingAudioManagerBackup = GetComponent<AudioManager> ();

		if (mBackgroundPrefab != null) 
		{
			mBackground = GameObject.Instantiate (mBackgroundPrefab);
		}
		else
		{
			mBackground = new GameObject("Menu Background");
		}

        fmodMusic = AudioManager.Instance.GetMusicEvent("MenuMusic/DroneMeny", false);
        fmodSwipe = AudioManager.Instance.GetSoundsEvent("MenuSectionSwipe/MenuSwipeShort", true);

		mGameMenus = GetComponentsInChildren<GameMenu> ();

		mCurrentGameMenu = mGameMenus [0];
	}

	// Use this for initialization
	void Start () 
	{
        System.GC.Collect();
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

		mBackground.gameObject.SetActive (show);
		gameObject.SetActive (show);
	}

	public void Disable() 
	{
		ShowComponents(false);

		AudioManager.Instance.StopMusic(fmodMusic);
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

		UpdateMenusAndButtons ();
	}

	public void UpdateMenusAndButtons()
	{
        bool viewBlocked = (MenuCamera.Instance.ShowingControls());
        bool menuOpened = (mShowOptionsMenu || mShowHelpMenu || mShowPopupAchievementsMenu || mShowPopupCraftingMenu);

        for (int i = 0; i < mGameMenus.Length; i++)
        {
            bool focus = ((mGameMenus[i] == mCurrentGameMenu) && !(viewBlocked) && (!menuOpened) && (!mMenuChangePhase));

            mGameMenus[i].SetFocus(focus);
        }

        MenuCamera.Instance.ShowHelpMenu(mShowHelpMenu);
        MenuGUICanvas.Instance.ShowHelpButtons(mShowHelpMenu);
		
		MenuCamera.Instance.ShowOptionsMenu(mShowOptionsMenu);
		MenuGUICanvas.Instance.ShowOptionButtons(mShowOptionsMenu);

		MenuCamera.Instance.ShowPopupCraftingMenu(mShowPopupCraftingMenu);
		MenuGUICanvas.Instance.ShowPopupCraftingButton(mShowPopupCraftingMenu);

		MenuCamera.Instance.ShowPopupAchievementsMenu(mShowPopupAchievementsMenu);
		MenuGUICanvas.Instance.ShowPopupAchievementsButton(mShowPopupAchievementsMenu);

		bool showBack = ((WorldMapMenu() != mCurrentGameMenu) && (!mMenuChangePhase));
        MenuCamera.Instance.ShowBackButton(showBack);
        MenuGUICanvas.Instance.IconsGUI().ShowWorldMapButton(showBack && (!viewBlocked));

        bool showIcons = ((!(mShowOptionsMenu || mShowHelpMenu)) && (!viewBlocked));
        MenuGUICanvas.Instance.ShowIconButtons(showIcons);

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
            // close popup meny
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
		ResetAllMenusAndButtons ();

        if (mCurrentGameMenu == mGameMenus[(int)state])
		{
			if (mMenuChangePhase)
			{
				// switch
				GameMenu a = mNextGameMenu;
				mNextGameMenu = mCurrentGameMenu;
				mCurrentGameMenu = a;
				MenuCamera.Instance.StartMenuMove (mNextGameMenu.gameObject);
			}
			else
			{
				// don't move if already there
			}

			UpdateMenusAndButtons();
			return; 
		}

		mMenuChangePhase = true;
        mNextGameMenu = mGameMenus[(int)state];
		mNextGameMenu.gameObject.SetActive (true);

		MenuCamera.Instance.StartMenuMove (mNextGameMenu.gameObject);
		AudioManager.Instance.PlaySound (fmodSwipe);

		UpdateMenusAndButtons();
	}
	
	void EndChangeGameMenu ()
	{
		mMenuChangePhase = false;
	
		mCurrentGameMenu.gameObject.SetActive (false);

		mCurrentGameMenu = mNextGameMenu;
		mNextGameMenu = null;

		AudioManager.Instance.StopSound(fmodSwipe);
		
		UpdateMenusAndButtons();
	}

    public void Deselect()
    {
        PlayerData.Instance.mShowControls = false;
        MenuCamera.Instance.ShowControls(false);
        if (mMenuChangePhase)
        {
            MenuCamera.Instance.Skip();
        }
        else
        {
            mCurrentGameMenu.Deselect();
        }
    }
}
