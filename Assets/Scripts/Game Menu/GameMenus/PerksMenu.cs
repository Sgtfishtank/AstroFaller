using UnityEngine;
using System.Collections;

public class PerksMenu : GameMenu 
{
	//private int AIR_PERK_INDEX = 1;
	//private int LIFE_PERK_INDEX = 2;
	//private int BURST_PERK_INDEX = 0;

	private Perk[] mPerks;
	private Perk mCurrentPerk;
	private bool mFocused;
	private int mPerkIndex;
	private GameObject mPrevObj;
	private GameObject mNextObj;

	void Awake()
	{
		mPrevObj = transform.Find("Prev").gameObject;
		mNextObj = transform.Find("Next").gameObject;
		mPerks = GetComponentsInChildren<Perk> ();
		mPerkIndex = 1; // start in the middle
	}

	// Use this for initialization
	void Start () 
	{
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
		GUICanvas.Instance.MenuGUICanvas().ShowPerkButtons(mFocused && (!MenuCamera.Instance.mCotrls.activeSelf) && (!MenuCamera.Instance.PopupBuyMenu().IsOpen()));
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
		mPrevObj.SetActive (mPerkIndex > 0);
		mNextObj.SetActive (mPerkIndex < (mPerks.Length - 1));
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

	public void BuyAirPerk ()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(mPerkIndex);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyLifePerk ()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(mPerkIndex);
		
		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
	
	public void BuyBurstPerk ()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons();
		
		OpenBuyPerkMenu(mPerkIndex);

		MainGameMenu.Instance.UpdateMenusAndButtons();
	}
}
