using UnityEngine;
using System.Collections;

public abstract class GameMenu : MonoBehaviour 
{
	public abstract void Init();
	
	public abstract void Focus();

	public abstract void Unfocus();
}
