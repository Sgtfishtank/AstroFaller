using UnityEngine;
using System.Collections;

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
				GameObject thisObject = GameObject.Find("Canvas");
				instance = thisObject.GetComponent<GUICanvas>();
			}
			return instance;
		}
	}

	private GameObject mWorldMapButton;
	private GameObject mPopupBuyMenu;
	private GameObject mPopupCraftingMenu;
	private GameObject mPopupAchievementsMenu;
	private GameObject mItemButtons;
	private GameObject mPerkButtons;

	// Use this for initialization
	void Start () 
	{
		mWorldMapButton = transform.Find ("WorldMapButton").gameObject;
		mPopupBuyMenu = transform.Find ("PopupBuyMenu").gameObject;
		mPopupCraftingMenu = transform.Find ("PopupCraftingMenu").gameObject;
		mPopupAchievementsMenu = transform.Find ("PopupAchievementsMenu").gameObject;
		mItemButtons = transform.Find ("Items").gameObject;
		mPerkButtons = transform.Find ("Perks").gameObject;

		ShowBackButton(false);
		ShowPopupBuyButton(false);
		ShowPopupCraftingButton(false);
		ShowPopupAchievementsButton(false);
		ShowItemButtons(false);
		ShowPerkButtons(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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

	// toggle buttons
	public void ShowBackButton (bool show)
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
