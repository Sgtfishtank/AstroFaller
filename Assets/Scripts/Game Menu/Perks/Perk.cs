using UnityEngine;
using System.Collections;

public abstract class Perk : MonoBehaviour 
{
	public abstract GameObject PreviewObject();

	public enum PerkPart
	{
		Main = 0,
		Left = 1,
		Right = 2
	}

	public abstract bool UnlockPart ();

	//public abstract bool IsPartUnlocked ();
	
	public abstract string BuyDescription ();
	
	public abstract string BuyCurrent ();
	
	public abstract string BuyNext ();
	
	public abstract int BuyCostBolts();

	public abstract int BuyCostCrystals();

	public abstract bool CanUnlockPart();
}
