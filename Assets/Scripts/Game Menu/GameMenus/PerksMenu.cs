using UnityEngine;
using System.Collections;

public class PerksMenu : GameMenu 
{
	private int AIR_PERK_INDEX = 1;
	private int LIFE_PERK_INDEX = 2;
	private int BURST_PERK_INDEX = 0;

	private Perk[] mPerks;
	private Perk mCurrentPerk;
	//private Perk.PerkPart mCurrentPerkPart;
	private bool mFocused;

	void Awake()
	{
		mPerks = GetComponentsInChildren<Perk> ();
	}

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (MenuCamera.Instance.PopupBuyMenu().IsOpen ()) 
		{
			string description = mCurrentPerk.BuyDescription ();
			string current = mCurrentPerk.BuyCurrent ();
			string next = mCurrentPerk.BuyNext ();
			int costBolts = mCurrentPerk.BuyCostBolts();
			int nextCrystals = mCurrentPerk.BuyCostCrystals();
			
			MenuCamera.Instance.PopupBuyMenu().updateData (mCurrentPerk.name, description, current, next, costBolts, nextCrystals);
		}
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

		if (mCurrentPerk != null)
		{
			CloseBuyPerkMenu ();
		}
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
	}
	
	public override void UpdateMenusAndButtons ()
	{		
		GUICanvas.Instance.MenuGUICanvas().ShowPerkButtons(mFocused && false && (!MenuCamera.Instance.PopupBuyMenu().IsOpen()));
	}
	
	public override void BuyWithBolts()
	{
		if (mCurrentPerk == null)
		{
			print("Error popup buying nothing!");
			CloseBuyPerkMenu();
			return;
		}
		
		int cost = mCurrentPerk.BuyCostBolts();
		if (PlayerData.Instance.withdrawBolts(cost))
		{
			mCurrentPerk.UnlockPart();
			CloseBuyPerkMenu();
			MainGameMenu.Instance.UpdateMenusAndButtons();
		}
	}
	
	public override void BuyWithCrystals()
	{
		if (mCurrentPerk == null)
		{
			print("Error popup buying nothing!");
			CloseBuyPerkMenu();
			return;
		}
		
		int cost = mCurrentPerk.BuyCostCrystals();
		if (PlayerData.Instance.withdrawCrystals(cost))
		{
			mCurrentPerk.UnlockPart();
			CloseBuyPerkMenu();
			MainGameMenu.Instance.UpdateMenusAndButtons();
		}

	}

	void OpenBuyPerkMenu(int index, Perk.PerkPart perkPart)
	{
		mCurrentPerk = mPerks[index];
		//mCurrentPerkPart = perkPart;

		// alredy unlocked or cannot unlock - ABORT! ABORT!!
		if ((mCurrentPerk.IsPartUnlocked()) || (!mCurrentPerk.CanUnlockPart()))
		{
			mCurrentPerk = null;
			return;
		}

		MenuCamera.Instance.PopupBuyMenu().Open(mCurrentPerk.PreviewObject());
	}
	
	void CloseBuyPerkMenu()
	{
		mCurrentPerk = null;
		MenuCamera.Instance.PopupBuyMenu().Close();
	}

	public void BuyAirPerk (Perk.PerkPart pp)
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(AIR_PERK_INDEX, pp);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyLifePerk (Perk.PerkPart pp)
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(LIFE_PERK_INDEX, pp);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyBurstPerk (Perk.PerkPart pp)
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(BURST_PERK_INDEX, pp);

		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
}
