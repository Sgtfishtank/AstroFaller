using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class MenuGUICanvas : GUICanvasBase 
{
	// snigleton
	private static MenuGUICanvas instance = null;
	public static MenuGUICanvas Instance
	{
		get
        {
			if (instance == null)
            {
                if (PlayerData.Instance.CurrentScene() != PlayerData.Scene.MAIN_MENU)
                {
                    throw new NotImplementedException();
                }

                instance = Singleton<MenuGUICanvas>.CreateInstance("Prefab/Game Menu/MenuGUICanvas");
			}
			return instance;
		}
	}

	private OptionsGUICanvas mOptionsGUICanvas;
    private DebugGUI mDebugGUI;
    private WorldMapGUI mMenuButtons;
    private PopupBuyGUI mPopupBuyMenu;
    private PopupCraftingGUI mPopupCraftingMenu;
    private PopupAchievementsGUI mPopupAchievementsMenu;
    private ItemsGUI mItemButtons;
    private PerksGUI mPerkButtons;
    private IconsGUI mIconButtons;

    private Image mFadeImage;
    private bool mShowButtons;
	private Button[] mButtons;

	void Awake ()
	{
        mFadeImage = transform.Find("FadeLayer").GetComponent<Image>();

        //assign all menu buttons
        mMenuButtons = transform.Find("MenuButtons").GetComponent<WorldMapGUI>();
        mPopupBuyMenu = transform.Find("PopupBuyMenu").GetComponent<PopupBuyGUI>();
        mPopupCraftingMenu = transform.Find("PopupCraftingMenu").GetComponent<PopupCraftingGUI>();
        mPopupAchievementsMenu = transform.Find("PopupAchievementsMenu").GetComponent<PopupAchievementsGUI>();
		mItemButtons = transform.Find("Items").GetComponent<ItemsGUI>();
		mPerkButtons = transform.Find("Perks").GetComponent<PerksGUI>();
        mIconButtons = transform.Find("Icons").GetComponent<IconsGUI>();
        mOptionsGUICanvas = transform.Find("Options GUICanvas").GetComponent<OptionsGUICanvas>();

        mButtons = GetComponentsInChildren<Button>(true);
		mDebugGUI = GetComponent<DebugGUI>();
	}

	// Use this for initialization
	void Start ()
    {
		mDebugGUI.enabled = false;
		ShowButtons (false);
	}

	// toggle menu buttons
	public void ShowWorldMapButtons(bool show)
	{
		mMenuButtons.gameObject.SetActive (show);
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
		//ShowButtons(mOptionsGUICanvas.GetButtons(), mShowButtons);
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

    // toggle menu buttons
    public void Deselect()
    {
        MainGameMenu.Instance.ResetAllMenusAndButtons();
        PlayerData.Instance.mShowControls = false;
        MenuCamera.Instance.ShowControls(false);
        MainGameMenu.Instance.UpdateMenusAndButtons();
    }

	public IconsGUI IconsGUI ()
	{
		return mIconButtons;
	}

	public PerksGUI PerksGUI ()
	{
		return mPerkButtons;
	}

	public ItemsGUI ItemsGUI ()
	{
		return mItemButtons;
	}

    public PopupAchievementsGUI AchievementsMenu()
    {
        return mPopupAchievementsMenu;
    }

    public PopupCraftingGUI CraftingMenu()
    {
        return mPopupCraftingMenu;
    }

    public WorldMapGUI WorldMapMenu()
    {
        return mMenuButtons;
    }

    public Button[] GetButtons()
    {
        return mButtons;
    }

    public void SetFadeColor(Color col)
    {
        mFadeImage.color = col;
    }

    public void ShowIconButtons(bool show)
    {
        mIconButtons.gameObject.SetActive(show);
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

    public void ShowHelpButtons(bool show)
    {
        // do nothing
    }
}
