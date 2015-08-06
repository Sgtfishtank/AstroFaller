using UnityEngine;
using System.Collections;

public class PerksMenu : GameMenu 
{
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
		mPopupBuyMenu = GameObject.Find("Pop-up buy menu").GetComponent<PopupBuyMenu>();
		mPopupBuyMenu.Close ();

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
				OpenBuyPerkMenu(0, Perk.PerkPart.Left);
			}
			else if (Input.GetKey(KeyCode.Keypad8))
			{
				OpenBuyPerkMenu(0, Perk.PerkPart.Main);
			}
			else if (Input.GetKey(KeyCode.Keypad9))
			{
				OpenBuyPerkMenu(0, Perk.PerkPart.Right);
			}

			if (Input.GetKey(KeyCode.Keypad4))
			{
				OpenBuyPerkMenu(1, Perk.PerkPart.Left);
			}
			else if (Input.GetKey(KeyCode.Keypad5))
			{
				OpenBuyPerkMenu(1, Perk.PerkPart.Main);
			}
			else if (Input.GetKey(KeyCode.Keypad6))
			{
				OpenBuyPerkMenu(1, Perk.PerkPart.Right);
			}
			if (Input.GetKey(KeyCode.Keypad1))
			{
				OpenBuyPerkMenu(2, Perk.PerkPart.Left);
			}
			else if (Input.GetKey(KeyCode.Keypad2))
			{
				OpenBuyPerkMenu(2, Perk.PerkPart.Main);
			}
			else if (Input.GetKey(KeyCode.Keypad3))
			{
				OpenBuyPerkMenu(2, Perk.PerkPart.Right);
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.KeypadEnter))
			{
				BuyPerks(PlayerData.CashType.Bolts);
				CloseBuyPerkMenu();
			}
			else if (Input.GetKey(KeyCode.KeypadPlus))
			{
				BuyPerks(PlayerData.CashType.Crystals);
				CloseBuyPerkMenu();
			}
			else if (Input.GetKey(KeyCode.Keypad0))
			{
				CloseBuyPerkMenu();
			}

			string description = mCurrentPerk.BuyDescription(mCurrentPerkPart);
			string current = mCurrentPerk.BuyCurrent(mCurrentPerkPart);
			string next = mCurrentPerk.BuyNext(mCurrentPerkPart);
			int costBolts = mCurrentPerk.BuyCost(mCurrentPerkPart, PlayerData.CashType.Bolts);
			int nextCrystals = mCurrentPerk.BuyCost(mCurrentPerkPart, PlayerData.CashType.Crystals);

			mPopupBuyMenu.updateData(description, current, next, costBolts, nextCrystals);
		}
	}
	
	public void BuyPerks(PlayerData.CashType buyType)
	{
		if (mCurrentPerk == null)
		{
			print("Error popup buying nothing!");
			return;
		}
		
		int cost = mCurrentPerk.BuyCost(mCurrentPerkPart, buyType);
		if (PlayerData.Instance.withdraw(cost, buyType))
		{
			mCurrentPerk.UnlockPart(mCurrentPerkPart);
		}
	}
}
