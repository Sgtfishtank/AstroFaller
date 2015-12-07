using UnityEngine;
using System.Collections;

public class PerksMenu : GameMenu 
{
	private Perk[] mPerks;
	private Perk mCurrentPerk;
	private bool mFocused;
	private int mPerkIndex = 1; // start in the middle
	private ButtonManager mPrevObj;
	private ButtonManager mNextObj;
	private ButtonManager[] mPerkButtons;
	
	void Awake()
	{
		mPerks = GetComponentsInChildren<Perk> ();
		mPerkButtons = new ButtonManager[3];
	}

	// Use this for initialization
	void Start () 
	{
		PerksGUI gui = MenuGUICanvas.Instance.PerksGUI ();
		mPrevObj = ButtonManager.CreateButton(gameObject, "Prev", "PrevButton", gui);
		mNextObj = ButtonManager.CreateButton(gameObject, "Next", "NextButton", gui);
		
		mPerkButtons[0] = ButtonManager.CreateButton(gameObject, "Perks Burst/perk_burst/Anim_BurstPerk", "LifePerk/Button 4", gui);
		mPerkButtons[1] = ButtonManager.CreateButton(gameObject, "Perks Air/perk_air/Anim_AirPerk", "LifePerk/Button 4", gui);
		mPerkButtons[2] = ButtonManager.CreateButton(gameObject, "Perks Life/perk_life/Anim_LifePerk", "LifePerk/Button 4", gui);

		UpdateViewPerk ();
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
	}
	
	public override void Unfocus()
	{
		mFocused = false;

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
		MenuGUICanvas.Instance.ShowPerkButtons(mFocused && (!MenuCamera.Instance.mCotrls.activeSelf) && (!MenuCamera.Instance.PopupBuyMenu().IsOpen()));
	}
	
	public void ViewNextPerk()
	{
		mPerkIndex++;	
		UpdateViewPerk();
	}
	
	public void ViewPreviousPerk()
	{
		mPerkIndex--;	
		UpdateViewPerk();
	}

	void UpdateViewPerk ()
	{
		mPerkIndex = Mathf.Clamp(mPerkIndex, 0, mPerks.Length - 1);	

		for (int i = 0; i < mPerks.Length; i++) 
		{
			mPerks[i].gameObject.SetActive(false);
		}

		mPerks[mPerkIndex].gameObject.SetActive(true);
		mPrevObj.mObj.SetActive (mPerkIndex > 0);
		mNextObj.mObj.SetActive (mPerkIndex < (mPerks.Length - 1));
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

	void OpenBuyPerkMenu(int index)
	{
		mCurrentPerk = mPerks[index];

		// alredy unlocked or cannot unlock - ABORT! ABORT!!
		if (!mCurrentPerk.CanUnlockPart())
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

	public void BuyPerk()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(mPerkIndex);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}

}
