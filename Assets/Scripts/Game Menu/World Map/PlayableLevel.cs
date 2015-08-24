using UnityEngine;
using System.Collections;

public abstract class PlayableLevel : LevelBase 
{
	public abstract void Open();
	
	public abstract void Close();
}
