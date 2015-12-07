using UnityEngine;
using System.Collections;

public class CrystalShopMenu : GameMenu 
{
	private bool mFocused;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
	
	public override void Focus()
	{
		mFocused = true;
	}
	
	public override void Unfocus()
	{
		mFocused = false;
	}

	public override bool IsFocused ()
	{
		return mFocused;
	}
	
	public override void UpdateMenusAndButtons ()
	{
	}

	public override void BuyWithBolts()
	{
	}
	
	public override void BuyWithCrystals()
	{
	}
}
