using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUICanvas : MonoBehaviour 
{
	// snigleton
	private static GUICanvas instance = null;
	public static GUICanvas Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("GUICanvas");
				instance = thisObject.GetComponent<GUICanvas>();
			}
			return instance;
		}
	}
	
	private GameObject mMenuButtons;
	private GameObject mInGameButtons;
	private GameObject mWorldMapButton;
	private GameObject mPopupBuyMenu;
	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	private GameObject mItemButtons;
	private GameObject mPerkButtons;
	private GameObject mIconButtons;
	private GameObject mPlayLevelButton;
	private GameObject mBackToMenuButton;
	private Image mFadeImage;
	private bool mShowDebugGUI;

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
		
		//assign all in game buttons
		mInGameButtons = transform.Find ("InGameButtons").gameObject;
		mBackToMenuButton = mInGameButtons.transform.Find ("BackToMenuButton").gameObject;

	}

	// Use this for initialization
	void Start () 
	{
		ButtonPress[] buttonPresss = transform.GetComponentsInChildren<ButtonPress>(true);
		for (int i = 0; i < buttonPresss.Length; i++) 
		{
			buttonPresss[i].Init();	
		}

		/*showPlayLevelButton(false);
		ShowIconButtons (true);
		ShowWorldMapButton(false);
		ShowPopupBuyButton(false);
		ShowPopupCraftingButton(false);
		ShowPopupAchievementsButton(false);
		ShowItemButtons(false);
		ShowPerkButtons(false);
		ShowBackToMenuButton(false);*/
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.LeftShift)))
		{
			mShowDebugGUI = true;
		}
	}

	void OnGUI()
	{
		if (!mShowDebugGUI)
		{
			return;
		}

		int startX = 10;
		int startY = 10;

		GUI.Box(new Rect(10, 10, 200, 200), "Debug Window");
	 	startX += 10;
		startY += 24;
		
		GUI.Label (new Rect(startX, startY, 180, 24), "Time on level: " + WorldGen.Instance.LevelRunTime());
		startY += 24;

		GUI.Label (new Rect(startX, startY, 180, 24), "Bolts on level: " + WorldGen.Instance.Player().colectedBolts());
		startY += 24;

		GUI.Label (new Rect(startX, startY, 180, 24), "Cyrstals on level: " + WorldGen.Instance.Player().colectedCrystals());
		startY += 24;
		
		GUI.Label (new Rect(startX, startY, 180, 24), "Distance on level: " + WorldGen.Instance.Player().distance());
		startY += 24;

		GUI.Label (new Rect(startX, startY, 180, 24), "Bolts: " + PlayerData.Instance.crystals());
		startY += 24;
		
		GUI.Label (new Rect(startX, startY, 180, 24), "Cyrstals: " + PlayerData.Instance.crystals());
		startY += 24;
		
		GUI.Label (new Rect(startX, startY, 180, 24), "Bolts Totla: " + PlayerData.Instance.totalBolts());
		startY += 24;
		
		GUI.Label (new Rect(startX, startY, 180, 24), "Cyrstals Total: " + PlayerData.Instance.totalCrystals());
		startY += 24;

		GUI.Label (new Rect(startX, startY, 180, 24), "Distance total: " + PlayerData.Instance.totalDistance());
		startY += 24;
	}

	public void SetFadeColor(Color col)
	{
		mFadeImage.color = col;
	}

	// pressed buy perks
	public void BuyAirPerk(int partperk)
	{
		MainGameMenu.Instance.PerksMenu().BuyAirPerk((Perk.PerkPart)partperk);
	}

	public void BuyBurstPerk(int partperk)
	{
		MainGameMenu.Instance.PerksMenu().BuyBurstPerk((Perk.PerkPart)partperk);
	}
	
	public void BuyLifePerk(int partperk)
	{
		MainGameMenu.Instance.PerksMenu().BuyLifePerk((Perk.PerkPart)partperk);
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
	
	// pressed back to menu
	public void BackToMenu()
	{
		WorldGen.Instance.Disable();
		MainGameMenu.Instance.Enable();
	}

	// options
	public void MuteMaster()
	{
		AudioManager.Instance.MuteMaster(transform.Find("Master").GetComponent<Toggle>().isOn);
	}

	public void MuteSounds()
	{
		AudioManager.Instance.MuteSounds(transform.Find("Master 2").GetComponent<Toggle>().isOn);
	}

	public void MuteMusic()
	{
		AudioManager.Instance.MuteMusic(transform.Find("Master 1").GetComponent<Toggle>().isOn);
	}

	public void MasterLevel()
	{
		AudioManager.Instance.MasterLevel(transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void SoundsLevel()
	{
		AudioManager.Instance.SoundsLevel(transform.Find("Slider 2").GetComponent<Slider>().value);
	}
	
	public void MusicLevel()
	{
		AudioManager.Instance.MusicLevel(transform.Find("Slider 1").GetComponent<Slider>().value);
	}

	// menu toggle buttons
	public void ShowMenuButtons(bool show)
	{
		mMenuButtons.gameObject.SetActive (show);
	}

	public void ShowInGameButtons(bool show)
	{
		mInGameButtons.gameObject.SetActive (show);
	}

	public void showPlayLevelButton (bool show)
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
	
	// in-game toggle buttons
	public void ShowBackToMenuButton(bool show)
	{
		mBackToMenuButton.gameObject.SetActive (show);
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

		if (ret == null) 
		{
			print("ERORR! BUTTON OBJECT NOT FOUND: " + name);
		}

		return ret;
	}

	public ButtonPress PlayButton()
	{
		return mPlayLevelButton.GetComponent<ButtonPress>();
	}
}
