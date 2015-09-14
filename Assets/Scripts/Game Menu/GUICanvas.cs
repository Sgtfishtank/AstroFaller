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
	private GameObject mWorldMapButton;
	private GameObject mPopupBuyMenu;
	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	private GameObject mItemButtons;
	private GameObject mPerkButtons;
	private GameObject mIconButtons;
	private GameObject mPlayLevelButton;
	private GameObject mInGameButtons;
	private GameObject mBackToMenuButton;
	private GameObject mOptionButtons;
	private GameObject mMasterButtons;
	private GameObject mSoundButtons;
	private GameObject mMusicButtons;
	private GameObject mDeathMenu;
	private GameObject mRewardMenu;
	private GameObject mRewardTextMenu;

	private Image mFadeImage;

	public bool mShowDebugGUI;
	private bool mShowButtons;
	private int mDebugGUISizeY;

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
		
		//assign all option buttons
		mOptionButtons = transform.Find ("OptionButtons").gameObject;
		mMasterButtons = mOptionButtons.transform.Find("Master").gameObject;
		mSoundButtons = mOptionButtons.transform.Find("Sounds").gameObject;
		mMusicButtons = mOptionButtons.transform.Find("Music").gameObject;
		mOptionButtons.SetActive (true);
		mDeathMenu = GameObject.Find("DeathMenu");
		mRewardMenu = GameObject.Find("Rewards");
		mRewardTextMenu = GameObject.Find("RewardText");
		mDeathMenu.SetActive(false);

		//assign all in game buttons
		mInGameButtons = transform.Find ("InGameButtons").gameObject;
		mBackToMenuButton = mInGameButtons.transform.Find ("BackToMenuButton").gameObject;

		mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
		mButtons = GetComponentsInChildren<Button>(true);

		ShowButtons (false);
	}

	// Use this for initialization
	void Start () 
	{
		UpdateOptions ();

		for (int i = 0; i < mButtonPresss.Length; i++) 
		{
			mButtonPresss[i].Init();	
		}
	}

	void ShowButtons(bool show)
	{
		float alpha = 0;
		if (show)
		{
			alpha = 1;
		}

		for (int i = 0; i < mButtons.Length; i++) 
		{
			ColorBlock cb = mButtons[i].colors;
			
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
			
			mButtons[i].colors = cb;
		}

		mShowButtons = show;
	}

	public GameObject OptionButtons()
	{
		return mOptionButtons;
	}

	// Update is called once per frame
	void Update () 
	{
		if (mShowDebugGUI)
		{
			Player pl = InGame.Instance.Player();

			if (Input.GetKeyDown(KeyCode.I))
			{
				pl.mInvulnerable = !pl.mInvulnerable;
			}
			
			if (Input.GetKeyDown(KeyCode.D))
			{
				pl.Dash();
			}
			
			if (Input.GetKeyDown(KeyCode.Plus))
			{
				pl.PlayerHeal(1);
			}
			
			if (Input.GetKeyDown(KeyCode.Minus))
			{
				pl.PlayerDamage(1);
			}
			
			if (Input.GetKeyDown(KeyCode.O))
			{
				pl.mUseAirReg = !pl.mUseAirReg;
			}

			if (Input.GetKeyDown(KeyCode.P))
			{
				pl.mUseAirDrain = !pl.mUseAirDrain;
			}

			if (Input.GetKeyDown(KeyCode.B))
			{
				ShowButtons(!mShowButtons);
			}

			if (Input.GetKeyDown(KeyCode.K))
			{
				PlayerData.Instance.depositBolts(999);
			}

			if (Input.GetKeyDown(KeyCode.L))
			{
				PlayerData.Instance.withdrawBolts(PlayerData.Instance.bolts());
			}
		}

		if ((Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.LeftShift)))
		{
			mShowDebugGUI = !mShowDebugGUI;

			if (mShowDebugGUI == false)
			{
				ShowButtons(false);
			}
			InGame.Instance.Player().mInvulnerable = false;
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
		int size = 20;

		GUI.Box(new Rect(10, 10, 200, mDebugGUISizeY - startY), "Debug Window");
	 	startX += 10;
		startY += size;

		if (InGame.Instance.gameObject.activeInHierarchy) 
		{
			GUI.Label (new Rect (startX, startY, 180, size), "Level: " + InGame.Instance.CurrentLevel ());
			startY += size;

			GUI.Label (new Rect (startX, startY, 180, size), "Time on level: " + WorldGen.Instance.LevelRunTime ());
			startY += size;

			GUI.Label (new Rect (startX, startY, 180, size), "Bolts on level: " + WorldGen.Instance.Player ().colectedBolts ());
			startY += size;

			GUI.Label (new Rect (startX, startY, 180, size), "Cyrstals on level: " + WorldGen.Instance.Player ().colectedCrystals ());
			startY += size;
		
			GUI.Label (new Rect (startX, startY, 180, size), "Distance on level: " + WorldGen.Instance.Player ().distance ());
			startY += size;
		}

		GUI.Label (new Rect (startX, startY, 180, size), "Bolts: " + PlayerData.Instance.bolts());
		startY += size;

		GUI.Label (new Rect(startX, startY, 180, size), "Cyrstals: " + PlayerData.Instance.crystals());
		startY += size;
		
		GUI.Label (new Rect(startX, startY, 180, size), "Bolts Total: " + PlayerData.Instance.totalBolts());
		startY += size;
		
		GUI.Label (new Rect(startX, startY, 180, size), "Cyrstals Total: " + PlayerData.Instance.totalCrystals());
		startY += size;

		GUI.Label (new Rect(startX, startY, 180, size), "Distance total: " + PlayerData.Instance.totalDistance());
		startY += size;
		
		if (InGame.Instance.Player().gameObject.activeInHierarchy) 
		{
			GUI.Label (new Rect (startX, startY, 180, size), "Player Air: " + InGame.Instance.Player ().airAmount ());
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player HP: " + InGame.Instance.Player ().mLife);
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player Invurable: " + InGame.Instance.Player ().mInvulnerable);
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player Reg Air: " + InGame.Instance.Player ().mUseAirReg);
			startY += size;
			
			GUI.Label (new Rect (startX, startY, 180, size), "Player Drain Air: " + InGame.Instance.Player ().mUseAirDrain);
			startY += size;
		}

		mDebugGUISizeY = startY;
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
	public void ToggleBloom()
	{
		MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
		InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
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
		MainGameMenu.Instance.Enable(0);
		mDeathMenu.SetActive(false);
		mRewardMenu.SetActive(false);
		InGame.Instance.mDeathMenu.SetActive(false);
	}

	// options
	public void MuteMaster()
	{
		AudioManager.Instance.MuteMaster(mMasterButtons.transform.Find("Mute").GetComponent<Toggle>().isOn);
	}

	public void MuteSounds()
	{
		AudioManager.Instance.MuteSounds(mSoundButtons.transform.Find("Mute").GetComponent<Toggle>().isOn);
	}

	public void MuteMusic()
	{
		AudioManager.Instance.MuteMusic(mMusicButtons.transform.Find("Mute").GetComponent<Toggle>().isOn);
	}

	public void MasterLevel()
	{
		AudioManager.Instance.MasterLevel(mMasterButtons.transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void SoundsLevel()
	{
		AudioManager.Instance.SoundsLevel(mSoundButtons.transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void MusicLevel()
	{
		AudioManager.Instance.MusicLevel(mMusicButtons.transform.Find("Slider").GetComponent<Slider>().value);
	}
	
	public void AcceptOptions()
	{
		MainGameMenu.Instance.ShowOptions (false, true);
	}
	
	public void CancelOptions()
	{
		MainGameMenu.Instance.ShowOptions (false, false);
	}

	//
	public void UpdateOptions()
	{
		mMusicButtons.transform.Find("Mute").GetComponent<Toggle>().isOn = AudioManager.Instance.IsMuteMusic();
		mSoundButtons.transform.Find("Mute").GetComponent<Toggle>().isOn = AudioManager.Instance.IsMuteSounds();
		mMasterButtons.transform.Find("Mute").GetComponent<Toggle>().isOn = AudioManager.Instance.IsMuteMaster();
		mMusicButtons.transform.Find("Slider").GetComponent<Slider>().value = AudioManager.Instance.GetMusicLevel();
		mSoundButtons.transform.Find("Slider").GetComponent<Slider>().value = AudioManager.Instance.GetSoundsLevel();
		mMasterButtons.transform.Find("Slider").GetComponent<Slider>().value = AudioManager.Instance.GetMasterLevel();
	}

	// toggle menu buttons
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
	
	// toggle in-game buttons
	public void ShowBackToMenuButton(bool show)
	{
		mBackToMenuButton.gameObject.SetActive (show);
	}
	
	// toggle options buttons
	public void ShowOptionButtons(bool show)
	{
		mOptionButtons.gameObject.SetActive (show);
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
			ret = InGame.Instance.GUIObject(name);
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
	public void setEnableDeathMenu(bool a)
	{
		mDeathMenu.SetActive(a);

	}
	public void restart()
	{
		InGame.Instance.mDeathMenu.SetActive(false);
		mDeathMenu.SetActive(false);
		mRewardMenu.SetActive(false);
		InGame.Instance.StartGame();
	}
	public void perfectDistanceReward(int pos)
	{
		int box = InGame.Instance.mPlayer.CollectedPerfectDistances();

		int value = 0;

		Text[] a = mRewardTextMenu.GetComponents<Text>();
		RectTransform[] b = mRewardMenu.GetComponentsInChildren<RectTransform>();


		if (box < 4)
		{
			value = UnityEngine.Random.Range(20,51);
			switch (pos)
			{
			case 1:
				a[1].text = value.ToString();
				a[1].rectTransform.anchoredPosition = b[1].anchoredPosition;
				break;
			case 2:
				a[2].text = value.ToString();
				a[2].rectTransform.anchoredPosition = b[2].anchoredPosition;
				
				break;
			case 3:
				a[3].text = value.ToString();
				a[3].rectTransform.anchoredPosition = b[3].anchoredPosition;
				break;
			case 4:
				a[4].text = value.ToString();
				a[4].rectTransform.anchoredPosition = b[4].anchoredPosition;
				break;
			default:
				break;
			}
		}
		else
		{
			for (int i = 0; i < box; i++)
			{
				value += UnityEngine.Random.Range(20,51);
			}
		}
	}
}
