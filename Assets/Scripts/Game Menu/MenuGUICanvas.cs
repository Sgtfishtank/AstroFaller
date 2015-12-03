using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MenuGUICanvas : MonoBehaviour 
{
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
	private ButtonPress[] mButtonPresss;
	
	void Awake () 
	{
		mFadeImage = transform.Find ("FadeLayer").GetComponent<Image> ();
		
		//assign all menu buttons
		mMenuButtons = transform.Find ("MenuButtons").gameObject;
		mWorldMapButton = mMenuButtons.transform.Find ("WorldMapButton").gameObject;
		mPopupBuyMenu = mMenuButtons.transform.Find ("PopupBuyMenu").gameObject;
		mPopupCraftingMenu = mMenuButtons.transform.Find ("PopupCraftingMenu").gameObject;
		mPopupAchievementsMenu = mMenuButtons.transform.Find ("PopupAchievementsMenu").gameObject;
		mItemButtons = mMenuButtons.transform.Find ("Items").gameObject;
		mPerkButtons = mMenuButtons.transform.Find ("Perks").gameObject;
		mIconButtons = mMenuButtons.transform.Find ("Icons").gameObject;
		mPlayLevelButton = mMenuButtons.transform.Find ("PlayLevelButton").gameObject;

		mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
		mButtons = GetComponentsInChildren<Button>(true);
	}

	// Use this for initialization
	void Start () 
	{
        if (!PlayerData.Instance.mShowControls)
        {
            MenuCamera.Instance.mCotrls.SetActive(false);
        }

		for (int i = 0; i < mButtonPresss.Length; i++) 
		{
			mButtonPresss[i].Init();	
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	// toggle menu buttons
	public void Deselect()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons ();
        MenuCamera.Instance.mCotrls.SetActive(false);
        PlayerData.Instance.mShowControls = false;
		MainGameMenu.Instance.UpdateMenusAndButtons ();
	}

	public Button[] GetButtons ()
	{
		return mButtons;
	}

	public void SetFadeColor(Color col)
	{
		mFadeImage.color = col;
	}

	// pressed buy perks
	public void BuyAirPerk()
	{
		MainGameMenu.Instance.PerksMenu().BuyAirPerk();
	}
	
	public void BuyBurstPerk()
	{
		MainGameMenu.Instance.PerksMenu().BuyBurstPerk();
	}
	
	public void BuyLifePerk()
	{
		MainGameMenu.Instance.PerksMenu().BuyLifePerk();
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
	public void ToggleOptions ()
	{
		MainGameMenu.Instance.ToggleOptions();
	}
	
	public void ToggleHelp ()
	{
		MainGameMenu.Instance.ToggleHelp();
	}
	
	public void ToggleCraftingMenu ()
	{
		MainGameMenu.Instance.ToggleCraftingMenu();
	}
	
	public void ToggleAchievementsMenu ()
	{
		MainGameMenu.Instance.ToggleAchievementsMenu();
	}
	
	// pressed popup buy buttons
	public void BuyWithBolts()
	{
		MainGameMenu.Instance.BuyWithBolts ();
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

	public void ShowPlayLevelButton (bool show)
	{
		mPlayLevelButton.gameObject.SetActive (show);
	}
	
	public void ShowIconButtons(bool show)
	{
		mIconButtons.gameObject.SetActive (show);
	}
	
	public void ShowWorldMapButton (bool show)
	{
		mWorldMapButton.gameObject.SetActive (show);
	}
	
	public void ShowPopupBuyButton (bool show)
	{
		mPopupBuyMenu.gameObject.SetActive (show);
	}
	
	public void ShowPopupCraftingButton (bool show)
	{
		mPopupCraftingMenu.gameObject.SetActive (show);
	}
	
	public void ShowPopupAchievementsButton (bool show)
	{
		mPopupAchievementsMenu.gameObject.SetActive (show);
	}
	
	public void ShowItemButtons (bool show)
	{
		mItemButtons.gameObject.SetActive (show);
	}
	
	public void ShowPerkButtons (bool show)
	{
		mPerkButtons.gameObject.SetActive (show);
	}

}
