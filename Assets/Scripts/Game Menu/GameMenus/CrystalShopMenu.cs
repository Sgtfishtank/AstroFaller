using UnityEngine;
using System.Collections;

public class CrystalShopMenu : GameMenu 
{
	private bool mFocused;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	public override void Init() 
	{
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
	}
	
	public override void BuyWithBolts()
	{
	}
	
	public override void BuyWithCrystals()
	{
	}
}
