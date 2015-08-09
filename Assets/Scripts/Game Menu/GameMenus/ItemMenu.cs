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
	private PopupBuyMenu mPopupBuyMenu;
	private Item mCurrentItem;

	// Use this for initialization
	void Start () 
	{
	}

	public override void Init() 
	{
		mPopupBuyMenu = MainGameMenu.Instance.PopupBuyMenu();

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
		if (!mPopupBuyMenu.IsOpen()) 
		{
			if (Input.GetKey(KeyCode.Keypad7))
			{
				OpenBuyItemMenu(UNLIMITED_AIR_INDEX);
			}

			else if (Input.GetKey(KeyCode.Keypad9))
			{
				OpenBuyItemMenu(SHOCKWAVE_INDEX);
			}
			
			if (Input.GetKey(KeyCode.Keypad4))
			{
				OpenBuyItemMenu(BOLTS_MAGNETS_INDEX);
			}

			else if (Input.GetKey(KeyCode.Keypad6))
			{
				OpenBuyItemMenu(FORCE_FIELD_INDEX);
			}
			if (Input.GetKey(KeyCode.Keypad1))
			{
				OpenBuyItemMenu(BOLTS_MULTIPLIER_INDEX);
			}

			else if (Input.GetKey(KeyCode.Keypad3))
			{
				OpenBuyItemMenu(ROCKET_THRUST_INDEX);
			}
		}
		else
		{
			if (Input.GetKey(KeyCode.KeypadEnter))
			{
				BuyWithBolts();
				CloseBuyItemMenu();
			}
			else if (Input.GetKey(KeyCode.KeypadPlus))
			{
				BuyWithCrystals();
				CloseBuyItemMenu();
			}
			else if (Input.GetKey(KeyCode.Keypad0))
			{
				CloseBuyItemMenu();
			}
		}
		
		if (mPopupBuyMenu.IsOpen ()) 
		{
			string description = mCurrentItem.BuyDescription ();
			string current = mCurrentItem.BuyCurrent ();
			string next = mCurrentItem.BuyNext ();
			int costBolts = mCurrentItem.BuyCostBolts();
			int nextCrystals = mCurrentItem.BuyCostCrystals();
			
			mPopupBuyMenu.updateData (description, current, next, costBolts, nextCrystals);
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
		
		for (int i = 0; i < mItems.Length; i++) 
		{
			TextMesh[] textMeshes = mItems[i].GetComponentsInChildren<TextMesh> ();
			for (int j = 0; j < textMeshes.Length; j++) 
			{
				textMeshes[j].gameObject.SetActive(false);
			}
		}
		
		mPopupBuyMenu.Open(transform.position);
	}

	public void BuyUlimitedAirItem()
	{
		OpenBuyItemMenu(UNLIMITED_AIR_INDEX);
	}
	
	public void BuyChockWaveItem()
	{
		OpenBuyItemMenu(SHOCKWAVE_INDEX);
	}
	
	public void BuyMagnetsItem()
	{
		OpenBuyItemMenu(BOLTS_MAGNETS_INDEX);
	}
	
	public void BuyForceFieldItem()
	{
		OpenBuyItemMenu(FORCE_FIELD_INDEX);
	}
	
	public void BuyMultiplierItem()
	{
		OpenBuyItemMenu(BOLTS_MULTIPLIER_INDEX);
	}
	
	public void BuyRocketThrustItem()
	{
		OpenBuyItemMenu(ROCKET_THRUST_INDEX);
	}

	void CloseBuyItemMenu()
	{
		mCurrentItem = null;
		
		for (int i = 0; i < mItems.Length; i++) 
		{
			TextMesh[] textMeshes = mItems[i].GetComponentsInChildren<TextMesh>(true);
			for (int j = 0; j < textMeshes.Length; j++) 
			{
				textMeshes[j].gameObject.SetActive(true);
			}
		}
		
		mPopupBuyMenu.Close();
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
		}
	}
}
