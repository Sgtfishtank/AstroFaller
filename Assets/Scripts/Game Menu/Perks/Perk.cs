using UnityEngine;
using System.Collections;

public abstract class Perk : MonoBehaviour 
{
	public enum PerkPart
	{
		Main = 0,
		Left = 1,
		Right = 2
	}
	
	public abstract void Init();

	public abstract bool UnlockPart (PerkPart perkPart);

	public abstract bool IsPartUnlocked (PerkPart perkPart);
	
	public abstract string BuyDescription (PerkPart perkPart);
	
	public abstract string BuyCurrent (PerkPart perkPart);
	
	public abstract string BuyNext (PerkPart perkPart);
	
	public abstract int BuyCostBolts(PerkPart perkPart);

	public abstract int BuyCostCrystals(PerkPart perkPart);

	public abstract bool CanUnlockPart(Perk.PerkPart perkPart);
}
