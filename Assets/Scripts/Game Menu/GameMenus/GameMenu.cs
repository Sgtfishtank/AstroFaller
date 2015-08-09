using UnityEngine;
using System.Collections;

public abstract class GameMenu : MonoBehaviour 
{
	public abstract void Init();
	
	public abstract void Focus();

	public abstract void Unfocus();

	public abstract bool IsFocused();

	public abstract void BuyWithBolts ();

	public abstract void BuyWithCrystals ();
}
