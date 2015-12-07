using UnityEngine;
using System.Collections;

public class ItemMenu : GameMenu 
{
	private int UNLIMITED_AIR_INDEX = 1;
	private int SHOCKWAVE_INDEX = 2;
	private int BOLTS_MAGNETS_INDEX = 4;
	private int FORCE_FIELD_INDEX = 3;
	private int BOLTS_MULTIPLIER_INDEX = 5;
	private int ROCKET_THRUST_INDEX = 0;

	private Item[] mItems;
	private ButtonManager[] mItemButtons;
	private bool mFocused;
	private Item mCurrentItem;

	void Awake() 
	{
		mItems = GetComponentsInChildren<Item> ();
		mItemButtons = new ButtonManager[6];
	}

	// Use this for initialization
	void Start () 
	{
		ItemsGUI gui = MenuGUICanvas.Instance.ItemsGUI();
		mItemButtons[UNLIMITED_AIR_INDEX] = ButtonManager.CreateButton(gameObject, "Unlimited Air/item_unlimitedair/item_unlimitedair", "UnlimitedAir", gui);
		mItemButtons[ROCKET_THRUST_INDEX] = ButtonManager.CreateButton(gameObject, "Rocket Thrust/item_megaburst/item_megaburst", "RocketThrust", gui);
		mItemButtons[SHOCKWAVE_INDEX] = ButtonManager.CreateButton(gameObject, "Shockwave/item_shockwave/item_shockwave", "Shockwave", gui);
		mItemButtons[FORCE_FIELD_INDEX] = ButtonManager.CreateButton(gameObject, "Force Field/item_shield/item_shield","ForceField", gui);
		mItemButtons[BOLTS_MAGNETS_INDEX] = ButtonManager.CreateButton(gameObject, "Bolt Magnet/item_boltmagnet/item_boltmagnet", "BoltsMagnet", gui);
		mItemButtons[BOLTS_MULTIPLIER_INDEX] = ButtonManager.CreateButton(gameObject, "Bolt Multiplier/item_boltmultiplier/item_boltmultiplier", "BoltsMultiplier", gui);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if ((mCurrentItem != null) && MenuCamera.Instance.PopupBuyMenu().IsOpen()) 
		{
			string description = mCurrentItem.BuyDescription ();
			string current = mCurrentItem.BuyCurrent ();
			string next = mCurrentItem.BuyNext ();
			int costBolts = mCurrentItem.BuyCostBolts();
			int nextCrystals = mCurrentItem.BuyCostCrystals();
			
			MenuCamera.Instance.PopupBuyMenu().updateData (mCurrentItem.name, description, current, next, costBolts, nextCrystals);
		}
	}

	public override void Focus()
	{
		mFocused = true;
	}
	
	public override void Unfocus()
	{
		mFocused = false;
		
		if (mCurrentItem != null)
		{
			CloseBuyItemMenu ();
		}
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
	}
	
	public override void UpdateMenusAndButtons ()
	{
		MenuGUICanvas.Instance.ShowItemButtons(mFocused && (!MenuCamera.Instance.mCotrls.activeSelf) && (!MenuCamera.Instance.PopupBuyMenu().IsOpen()));
	}
	
	public override void BuyWithBolts()
	{
		if (mCurrentItem == null)
		{
			print("Error popup buying nothing!");
			return;
		}
		
		int cost = mCurrentItem.BuyCostBolts();
		if (PlayerData.Instance.withdrawBolts(cost))
		{
			mCurrentItem.UnlockItem();
			CloseBuyItemMenu ();
			MainGameMenu.Instance.UpdateMenusAndButtons();
		}
	}
	
	public override void BuyWithCrystals()
	{
		if (mCurrentItem == null)
		{
			print("Error popup buying nothing!");
			return;
		}
		
		int cost = mCurrentItem.BuyCostCrystals();
		if (PlayerData.Instance.withdrawCrystals(cost))
		{
			mCurrentItem.UnlockItem();
			CloseBuyItemMenu ();
			MainGameMenu.Instance.UpdateMenusAndButtons();
		}
	}

	void OpenBuyItemMenu(int index)
	{
		mCurrentItem = mItems[index];
		print ("i " + index + " n " + mCurrentItem.name);
		
		// cannot unlock more - ABORT! ABORT!!
		if (!mCurrentItem.CanUnlockItem())
		{
			mCurrentItem = null;
			return;
		}

		MenuCamera.Instance.PopupBuyMenu().Open(mItems[index].PreviewObject());
	}
	
	void CloseBuyItemMenu()
	{
		mCurrentItem = null;
		MenuCamera.Instance.PopupBuyMenu().Close();
	}
	
	public void BuyUlimitedAirItem()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyItemMenu(UNLIMITED_AIR_INDEX);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyShockwaveItem()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		OpenBuyItemMenu(SHOCKWAVE_INDEX);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyBoltMagnetItem()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		OpenBuyItemMenu(BOLTS_MAGNETS_INDEX);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyForceFieldItem()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		OpenBuyItemMenu(FORCE_FIELD_INDEX);
	}
	
	public void BuyBoltMultiplierItem()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		OpenBuyItemMenu(BOLTS_MULTIPLIER_INDEX);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyRocketThrustItem()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();

		OpenBuyItemMenu(ROCKET_THRUST_INDEX);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
}
