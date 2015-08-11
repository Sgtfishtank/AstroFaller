using UnityEngine;
using System.Collections;

public class ItemMenu : GameMenu 
{
	private int UNLIMITED_AIR_INDEX = 0;
	private int SHOCKWAVE_INDEX = 1;
	private int BOLTS_MAGNETS_INDEX = 2;
	private int FORCE_FIELD_INDEX = 3;
	private int BOLTS_MULTIPLIER_INDEX = 4;
	private int ROCKET_THRUST_INDEX = 5;

	private Item[] mItems;
	private bool mFocused;
	private Item mCurrentItem;

	// Use this for initialization
	void Start () 
	{
	}

	public override void Init() 
	{
		mItems = GetComponentsInChildren<Item> ();
		for (int i = 0; i < mItems.Length; i++) 
		{
			mItems[i].Init();
		}
		
		mFocused = false;
		enabled = false;
	}
	
	public override void Focus()
	{
		mFocused = true;
		enabled = true;
	}
	
	public override void Unfocus()
	{
		mFocused = false;
		enabled = false;
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
	}

	// Update is called once per frame
	void Update () 
	{
		if (MenuCamera.Instance.PopupBuyMenu().IsOpen ()) 
		{
			string description = mCurrentItem.BuyDescription ();
			string current = mCurrentItem.BuyCurrent ();
			string next = mCurrentItem.BuyNext ();
			int costBolts = mCurrentItem.BuyCostBolts();
			int nextCrystals = mCurrentItem.BuyCostCrystals();
			
			MenuCamera.Instance.PopupBuyMenu().updateData (description, current, next, costBolts, nextCrystals);
		}
	}
	
	void OpenBuyItemMenu(int index)
	{
		mCurrentItem = mItems[index];
		
		// alredy cannot unlock more - ABORT! ABORT!!
		if (!mCurrentItem.CanUnlockItem())
		{
			mCurrentItem = null;
			return;
		}

		MenuCamera.Instance.PopupBuyMenu().Open(transform.position);
		GUICanvas.Instance.HideItemButtons();
	}
	
	void CloseBuyItemMenu()
	{
		mCurrentItem = null;

		MenuCamera.Instance.PopupBuyMenu().Close();
		GUICanvas.Instance.ShowItemButtons();
	}

	public void BuyUlimitedAirItem()
	{
		MainGameMenu.Instance.ResetAllMenus ();
		OpenBuyItemMenu(UNLIMITED_AIR_INDEX);
	}
	
	public void BuyShockwaveItem()
	{
		MainGameMenu.Instance.ResetAllMenus ();
		OpenBuyItemMenu(SHOCKWAVE_INDEX);
	}
	
	public void BuyBoltMagnetItem()
	{
		MainGameMenu.Instance.ResetAllMenus ();
		OpenBuyItemMenu(BOLTS_MAGNETS_INDEX);
	}
	
	public void BuyForceFieldItem()
	{
		MainGameMenu.Instance.ResetAllMenus ();
		OpenBuyItemMenu(FORCE_FIELD_INDEX);
	}
	
	public void BuyBoltMultiplierItem()
	{
		MainGameMenu.Instance.ResetAllMenus ();
		OpenBuyItemMenu(BOLTS_MULTIPLIER_INDEX);
	}
	
	public void BuyRocketThrustItem()
	{
		MainGameMenu.Instance.ResetAllMenus ();
		OpenBuyItemMenu(ROCKET_THRUST_INDEX);
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
		}
	}
}
