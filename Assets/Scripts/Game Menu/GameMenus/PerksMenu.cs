using UnityEngine;
using System.Collections;

public class PerksMenu : GameMenu 
{
	private int AIR_PERK_INDEX = 2;
	private int LIFE_PERK_INDEX = 1;
	private int BURST_PERK_INDEX = 0;

	private Perk[] mPerks;
	private Perk mCurrentPerk;
	private Perk.PerkPart mCurrentPerkPart;
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
			string description = mCurrentPerk.BuyDescription (mCurrentPerkPart);
			string current = mCurrentPerk.BuyCurrent (mCurrentPerkPart);
			string next = mCurrentPerk.BuyNext (mCurrentPerkPart);
			int costBolts = mCurrentPerk.BuyCostBolts(mCurrentPerkPart);
			int nextCrystals = mCurrentPerk.BuyCostCrystals(mCurrentPerkPart);
			
			MenuCamera.Instance.PopupBuyMenu().updateData (description, current, next, costBolts, nextCrystals);
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
		GUICanvas.Instance.ShowPerkButtons(mFocused && (!MenuCamera.Instance.PopupBuyMenu().IsOpen()));
	}
	
	public override void BuyWithBolts()
	{
		if (mCurrentPerk == null)
		{
			print("Error popup buying nothing!");
			CloseBuyPerkMenu();
			return;
		}
		
		int cost = mCurrentPerk.BuyCostBolts(mCurrentPerkPart);
		if (PlayerData.Instance.withdrawBolts(cost))
		{
			mCurrentPerk.UnlockPart(mCurrentPerkPart);
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
		
		int cost = mCurrentPerk.BuyCostCrystals(mCurrentPerkPart);
		if (PlayerData.Instance.withdrawCrystals(cost))
		{
			mCurrentPerk.UnlockPart(mCurrentPerkPart);
			CloseBuyPerkMenu();
			MainGameMenu.Instance.UpdateMenusAndButtons();
		}

	}

	void OpenBuyPerkMenu(int index, Perk.PerkPart perkPart)
	{
		mCurrentPerk = mPerks[index];
		mCurrentPerkPart = perkPart;

		// alredy unlocked or cannot unlock - ABORT! ABORT!!
		if ((mCurrentPerk.IsPartUnlocked(perkPart)) || (!mCurrentPerk.CanUnlockPart(perkPart)))
		{
			mCurrentPerk = null;
			return;
		}

		MenuCamera.Instance.PopupBuyMenu().Open();
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
