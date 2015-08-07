using UnityEngine;
using System.Collections;

public abstract class Item : MonoBehaviour 
{
	public abstract void Init();

	public abstract bool UnlockItem ();
	
	public abstract bool IsUnlocked ();

	public abstract int ItemLevelUnlocked ();

	public abstract bool CanUnlockItem ();

	public abstract int BuyCostBolts ();

	public abstract int BuyCostCrystals ();
	
	public abstract string BuyDescription ();
	
	public abstract string BuyCurrent ();
	
	public abstract string BuyNext ();
}
