using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class MenuGUICanvas : MonoBehaviour 
{
	// snigleton
	private static MenuGUICanvas instance = null;
	public static MenuGUICanvas Instance
	{
		get
        {
            if (Application.loadedLevelName != "MainMenuLevel")
            {
                throw new NotImplementedException();
            }
            
			if (instance == null)
            {
                instance = Singleton<MenuGUICanvas>.CreateInstance("Prefab/Essential/Menu/MenuGUICanvas");
			}
			return instance;
		}
	}

	private OptionsGUICanvas mOptionsGUICanvas;
    private DebugGUI mDebugGUI;

    private GameObject mMenuGUICanvas;
    private GameObject mMenuButtons;
    private GameObject mWorldMapButton;
    private GameObject mPopupBuyMenu;
    private GameObject mPopupCraftingMenu;
    private GameObject mPopupAchievementsMenu;
    private GameObject mItemButtons;
    private GameObject mPerkButtons;
    private GameObject mIconButtons;
    private GameObject mPlayLevelButton;

    private Image mFadeImage;

    private bool mShowButtons;
    private Button[] mButtons;
    //private ButtonPress[] mButtonPresss;

	void Awake ()
    {
        mFadeImage = transform.Find("FadeLayer").GetComponent<Image>();

        //assign all menu buttons
        mMenuGUICanvas = transform.Find("MenuButtons").gameObject;
        mMenuButtons = transform.Find("MenuButtons").gameObject;
        mWorldMapButton = mMenuButtons.transform.Find("WorldMapButton").gameObject;
        mPopupBuyMenu = mMenuButtons.transform.Find("PopupBuyMenu").gameObject;
        mPopupCraftingMenu = mMenuButtons.transform.Find("PopupCraftingMenu").gameObject;
        mPopupAchievementsMenu = mMenuButtons.transform.Find("PopupAchievementsMenu").gameObject;
        mItemButtons = mMenuButtons.transform.Find("Items").gameObject;
        mPerkButtons = mMenuButtons.transform.Find("Perks").gameObject;
        mIconButtons = mMenuButtons.transform.Find("Icons").gameObject;
        mPlayLevelButton = mMenuButtons.transform.Find("PlayLevelButton").gameObject;

        //mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
        mButtons = GetComponentsInChildren<Button>(true);

		mDebugGUI = GetComponent<DebugGUI>();
		mOptionsGUICanvas = GetComponentsInChildren<OptionsGUICanvas>(true)[0];
		mOptionsGUICanvas.gameObject.SetActive (true);
	}

	// Use this for initialization
	void Start ()
    {
        if (!PlayerData.Instance.mShowControls)
        {
            MenuCamera.Instance.mCotrls.SetActive(false);
        }

		mDebugGUI.enabled = false;
		ShowButtons (false);
	}

	// toggle menu buttons
	public void ShowMenuButtons(bool show)
	{
		mMenuGUICanvas.gameObject.SetActive (show);
	}
	
	// toggle options buttons
	public void ShowOptionButtons(bool show)
	{
		mOptionsGUICanvas.gameObject.SetActive (show);
	}

	public void ToggleShowButtons ()
	{
		mShowButtons = !mShowButtons;
		UpdateButtons();
	}
	
	public void ShowButtons(bool show)
	{
		mShowButtons = show;
		UpdateButtons();
	}

	void UpdateButtons ()
	{
		ShowButtons(mOptionsGUICanvas.GetButtons(), mShowButtons);
		ShowButtons(mButtons, mShowButtons);
	}

	public OptionsGUICanvas OptionsGUICanvas ()
	{
		return mOptionsGUICanvas;
	}

	public void ShowButtons(Button[] buttons, bool show)
	{
		float alpha = 0;
		if (show)
		{
			alpha = 1;
		}

		for (int i = 0; i < buttons.Length; i++) 
		{
			ColorBlock cb = buttons[i].colors;
			
			Color col = cb.highlightedColor;
			col.a = alpha;
			cb.highlightedColor = col;
			
			col = cb.normalColor;
			col.a = alpha;
			cb.normalColor = col;
			
			col = cb.pressedColor;
			col.a = alpha;
			cb.pressedColor = col;
			
			col = cb.disabledColor;
			col.a = alpha;
			cb.disabledColor = col;
			
			buttons[i].colors = cb;
		}
	}

	// Update is called once per frame
	void Update () 
	{
		Input.multiTouchEnabled = true;
		bool debugTouch = (Input.touchCount >= 4) && (Input.touches [3].phase == TouchPhase.Began);
		bool debugKey = (Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.LeftShift));

		if (debugKey || debugTouch)
		{
			mDebugGUI.enabled = !mDebugGUI.enabled;
			
			if (!mDebugGUI.enabled)
			{
				MenuGUICanvas.Instance.ShowButtons(false);
				//InGame.Instance.Player().mInvulnerable = false;
			}
		}

	}

	public void ToggleBloom()
	{
		MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
		InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
	}

	// other
	public GameObject GUIObject (string name)
	{
		switch (name) 
		{
		case "a":
			return null;
		}

		GameObject ret = MenuCamera.Instance.GUIObject(name);
		if (ret == null)
		{
			ret = MainGameMenu.Instance.GUIObject(name);
		}

		/*if (ret == null)
		{
			ret = InGame.Instance.GUIObject(name);
		}*/

		if (ret == null) 
		{
			Debug.LogWarning("Warning! BUTTON OBJECT NOT FOUND: " + name);
		}

		return ret;
	}

    // toggle menu buttons
    public void Deselect()
    {
        MainGameMenu.Instance.ResetAllMenusAndButtons();
        MenuCamera.Instance.mCotrls.SetActive(false);
        PlayerData.Instance.mShowControls = false;
        MainGameMenu.Instance.UpdateMenusAndButtons();
    }

    public Button[] GetButtons()
    {
        return mButtons;
    }

    public void SetFadeColor(Color col)
    {
        mFadeImage.color = col;
    }

    // pressed buy perks
    public void BuyPerk()
    {
        MainGameMenu.Instance.PerksMenu().BuyPerk();
    }

    public void ViewNextPerk()
    {
        MainGameMenu.Instance.PerksMenu().ViewNextPerk();
    }

    public void ViewPreviousPerk()
    {
        MainGameMenu.Instance.PerksMenu().ViewPreviousPerk();
    }

    // pressed buy items
    public void BuyUlimitedAirItem()
    {
        MainGameMenu.Instance.ItemsMenu().BuyUlimitedAirItem();
    }

    public void BuyShockwaveItem()
    {
        MainGameMenu.Instance.ItemsMenu().BuyShockwaveItem();
    }

    public void BuyMagnetsItem()
    {
        MainGameMenu.Instance.ItemsMenu().BuyBoltMagnetItem();
    }

    public void BuyForceFieldItem()
    {
        MainGameMenu.Instance.ItemsMenu().BuyForceFieldItem();
    }

    public void BuyMultiplierItem()
    {
        MainGameMenu.Instance.ItemsMenu().BuyBoltMultiplierItem();
    }

    public void BuyRocketThrustItem()
    {
        MainGameMenu.Instance.ItemsMenu().BuyRocketThrustItem();
    }

    // pressed change world menus
    public void ChangeToWorldMapMenu()
    {
        MainGameMenu.Instance.ChangeToWorldMapMenu();
    }

    public void ChangeToPerksMenu()
    {
        MainGameMenu.Instance.ChangeToPerksMenu();
    }

    public void ChangeToItemsMenu()
    {
        MainGameMenu.Instance.ChangeToItemsMenu();
    }

    public void ChangeToChrystalShopMenu()
    {
        MainGameMenu.Instance.ChangeToChrystalShopMenu();
    }

    // pressed main gui buttons
    public void ToggleOptions()
    {
        MainGameMenu.Instance.ToggleOptions();
    }

    public void ToggleHelp()
    {
        MainGameMenu.Instance.ToggleHelp();
    }

    public void ToggleCraftingMenu()
    {
        MainGameMenu.Instance.ToggleCraftingMenu();
    }

    public void ToggleAchievementsMenu()
    {
        MainGameMenu.Instance.ToggleAchievementsMenu();
    }

    // pressed popup buy buttons
    public void BuyWithBolts()
    {
        MainGameMenu.Instance.BuyWithBolts();
    }

    public void BuyWithCrystals()
    {
        MainGameMenu.Instance.BuyWithCrystals();
    }

    // PlayLevel
    public void PlayLevel()
    {
        MainGameMenu.Instance.WorldMapMenu().PlayLevel();
    }

    // get buttons
    public ButtonPress PlayButton()
    {
        return mPlayLevelButton.GetComponent<ButtonPress>();
    }

    public void ShowPlayLevelButton(bool show)
    {
        mPlayLevelButton.gameObject.SetActive(show);
    }

    public void ShowIconButtons(bool show)
    {
        mIconButtons.gameObject.SetActive(show);
    }

    public void ShowWorldMapButton(bool show)
    {
        mWorldMapButton.gameObject.SetActive(show);
    }

    public void ShowPopupBuyButton(bool show)
    {
        mPopupBuyMenu.gameObject.SetActive(show);
    }

    public void ShowPopupCraftingButton(bool show)
    {
        mPopupCraftingMenu.gameObject.SetActive(show);
    }

    public void ShowPopupAchievementsButton(bool show)
    {
        mPopupAchievementsMenu.gameObject.SetActive(show);
    }

    public void ShowItemButtons(bool show)
    {
        mItemButtons.gameObject.SetActive(show);
    }

    public void ShowPerkButtons(bool show)
    {
        mPerkButtons.gameObject.SetActive(show);
    }
}
