using UnityEngine;
using System.Collections;

public abstract class Perk : MonoBehaviour 
{
	public enum PerkPart
	{
		Main,
		Left,
		Right
	}
	
	public abstract void Init();

	public abstract bool UnlockPart (PerkPart perkPart);

	public abstract bool IsPartUnlocked (PerkPart perkPart);
}
