using UnityEngine;
using System.Collections;

public class ItemMenu : GameMenu 
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

	// Update is called once per frame
	void Update () 
	{

	}
}
