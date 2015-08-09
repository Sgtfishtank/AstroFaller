using UnityEngine;
using System.Collections;

public class GUICanvas : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void BuyAirPerk(int partperk)
	{
		Perk.PerkPart pp = (Perk.PerkPart)partperk;
		MainGameMenu.Instance.PerksMenu().BuyAirPerk(pp);
	}
	
	public void BuyBurstPerk(int partperk)
	{
		Perk.PerkPart pp = (Perk.PerkPart)partperk;
		MainGameMenu.Instance.PerksMenu().BuyBurstPerk(pp);
	}
	
	public void BuyLifePerk(int partperk)
	{
		Perk.PerkPart pp = (Perk.PerkPart)partperk;
		MainGameMenu.Instance.PerksMenu().BuyLifePerk(pp);
	}

	public void BuyUlimitedAirItem()
	{
		MainGameMenu.Instance.ItemsMenu().BuyUlimitedAirItem();
	}
	
	public void BuyChockWaveItem()
	{
		MainGameMenu.Instance.ItemsMenu().BuyChockWaveItem();
	}
	
	public void BuyMagnetsItem()
	{
		MainGameMenu.Instance.ItemsMenu().BuyMagnetsItem();
	}
	
	public void BuyForceFieldItem()
	{
		MainGameMenu.Instance.ItemsMenu().BuyForceFieldItem();
	}
	
	public void BuyMultiplierItem()
	{
		MainGameMenu.Instance.ItemsMenu().BuyMultiplierItem();
	}
	
	public void BuyRocketThrustItem()
	{
		MainGameMenu.Instance.ItemsMenu().BuyRocketThrustItem();
	}

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
	
	public void BuyWithBolts()
	{
		MainGameMenu.Instance.BuyWithBolts ();
	}
	
	public void BuyWithCrystals()
	{
		MainGameMenu.Instance.BuyWithCrystals();
	}

	public void HideBackButton ()
	{
		transform.Find ("WorldMapButton").gameObject.SetActive (false);
	}

	public void ShowBackButton ()
	{
		transform.Find ("WorldMapButton").gameObject.SetActive (true);
	}

	public void HidePopupBuyButton ()
	{
		transform.Find ("PopupBuyMenu").gameObject.SetActive (false);
	}

	public void HidePopupCraftingButton ()
	{
		transform.Find ("PopupCraftingMenu").gameObject.SetActive (false);
	}
	
	public void HidePopupAchievementsButton ()
	{
		transform.Find ("PopupAchievementsMenu").gameObject.SetActive (false);
	}
	
	public void ShowPopupBuyButton ()
	{
		transform.Find ("PopupBuyMenu").gameObject.SetActive (true);
	}

	public void ShowPopupCraftingButton ()
	{
		transform.Find ("PopupCraftingMenu").gameObject.SetActive (true);
	}
	
	public void ShowPopupAchievementsButton ()
	{
		transform.Find ("PopupAchievementsMenu").gameObject.SetActive (true);
	}
	
	public void HideItemButtons ()
	{
		transform.Find ("Items").gameObject.SetActive (false);
	}

	public void ShowItemButtons ()
	{
		transform.Find ("Items").gameObject.SetActive (true);
	}

	public void HidePerkButtons ()
	{
		transform.Find ("Perks").gameObject.SetActive (false);
	}

	public void ShowPerkButtons ()
	{
		transform.Find ("Perks").gameObject.SetActive (true);
	}
}
