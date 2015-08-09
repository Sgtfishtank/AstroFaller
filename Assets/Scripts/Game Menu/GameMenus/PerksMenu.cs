using UnityEngine;
using System.Collections;

public class PerksMenu : GameMenu 
{
	private int AIR_PERK_INDEX = 0;
	private int LIFE_PERK_INDEX = 1;
	private int BURST_PERK_INDEX = 2;

	private Perk[] mPerks;
	private PopupBuyMenu mPopupBuyMenu;
	private Perk mCurrentPerk;
	private Perk.PerkPart mCurrentPerkPart;
	private bool mFocused;

	// Use this for initialization
	void Start () 
	{
	}

	public override void Init() 
	{
		mPopupBuyMenu = MainGameMenu.Instance.PopupBuyMenu();

		mPerks = GetComponentsInChildren<Perk> ();
		for (int i = 0; i < mPerks.Length; i++) 
		{
			mPerks[i].Init();
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

		CloseBuyPerkMenu ();
	}
	
	public override bool IsFocused ()
	{
		return mFocused;
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
		
		for (int i = 0; i < mPerks.Length; i++) 
		{
			TextMesh[] textMeshes = mPerks[i].GetComponentsInChildren<TextMesh> ();
			for (int j = 0; j < textMeshes.Length; j++) 
			{
				textMeshes[j].gameObject.SetActive(false);
			}
		}

		mPopupBuyMenu.Open(transform.position);
	}
	
	void CloseBuyPerkMenu()
	{
		mCurrentPerk = null;

		for (int i = 0; i < mPerks.Length; i++) 
		{
			TextMesh[] textMeshes = mPerks[i].GetComponentsInChildren<TextMesh>(true);
			for (int j = 0; j < textMeshes.Length; j++) 
			{
				textMeshes[j].gameObject.SetActive(true);
			}
		}

		mPopupBuyMenu.Close();
	}

	// Update is called once per frame
	void Update () 
	{
		if (!mPopupBuyMenu.IsOpen()) 
		{
			if (Input.GetKey(KeyCode.Keypad7))
			{
				OpenBuyPerkMenu(AIR_PERK_INDEX, Perk.PerkPart.Left);
			}
			else if (Input.GetKey(KeyCode.Keypad8))
			{
				OpenBuyPerkMenu(AIR_PERK_INDEX, Perk.PerkPart.Main);
			}
			else if (Input.GetKey(KeyCode.Keypad9))
			{
				OpenBuyPerkMenu(AIR_PERK_INDEX, Perk.PerkPart.Right);
			}

			if (Input.GetKey(KeyCode.Keypad4))
			{
				OpenBuyPerkMenu(LIFE_PERK_INDEX, Perk.PerkPart.Left);
			}
			else if (Input.GetKey(KeyCode.Keypad5))
			{
				OpenBuyPerkMenu(LIFE_PERK_INDEX, Perk.PerkPart.Main);
			}
			else if (Input.GetKey(KeyCode.Keypad6))
			{
				OpenBuyPerkMenu(LIFE_PERK_INDEX, Perk.PerkPart.Right);
			}
			if (Input.GetKey(KeyCode.Keypad1))
			{
				OpenBuyPerkMenu(BURST_PERK_INDEX, Perk.PerkPart.Left);
			}
			else if (Input.GetKey(KeyCode.Keypad2))
			{
				OpenBuyPerkMenu(BURST_PERK_INDEX, Perk.PerkPart.Main);
			}
			else if (Input.GetKey(KeyCode.Keypad3))
			{
				OpenBuyPerkMenu(BURST_PERK_INDEX, Perk.PerkPart.Right);
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.KeypadEnter))
			{
				BuyWithBolts();
			}
			else if (Input.GetKey(KeyCode.KeypadPlus))
			{
				BuyWithCrystals();
			}
			else if (Input.GetKey(KeyCode.Keypad0))
			{
				CloseBuyPerkMenu();
			}
		}

		if (mPopupBuyMenu.IsOpen ()) 
		{
			string description = mCurrentPerk.BuyDescription (mCurrentPerkPart);
			string current = mCurrentPerk.BuyCurrent (mCurrentPerkPart);
			string next = mCurrentPerk.BuyNext (mCurrentPerkPart);
			int costBolts = mCurrentPerk.BuyCostBolts(mCurrentPerkPart);
			int nextCrystals = mCurrentPerk.BuyCostCrystals(mCurrentPerkPart);

			mPopupBuyMenu.updateData (description, current, next, costBolts, nextCrystals);
		}
	}

	public void BuyAirPerk (Perk.PerkPart pp)
	{
		OpenBuyPerkMenu(AIR_PERK_INDEX, pp);
	}
	
	public void BuyLifePerk (Perk.PerkPart pp)
	{
		OpenBuyPerkMenu(LIFE_PERK_INDEX, pp);
	}
	
	public void BuyBurstPerk (Perk.PerkPart pp)
	{
		OpenBuyPerkMenu(BURST_PERK_INDEX, pp);
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
		}
		CloseBuyPerkMenu();
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
		}
		CloseBuyPerkMenu();
	}
}
