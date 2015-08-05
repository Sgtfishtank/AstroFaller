using UnityEngine;
using System.Collections;

public abstract class LevelBase : MonoBehaviour 
{
	public abstract void Init();

	public abstract bool UnlockLevel ();
	
	public abstract bool IsUnlocked ();
	
	public abstract bool LockLevel ();

	public abstract void setFocusLevel (float focusLevel);
}
