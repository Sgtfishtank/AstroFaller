using UnityEngine;
using System.Collections;

public class PerksGUI : GUICanvasBase 
{
	private ButtonPress mPerkButton;
	private ButtonPress mNextButton;
	private ButtonPress mPrevButton;

	void Awake()
	{
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void ViewNextPerk()
	{
		MainGameMenu.Instance.PerksMenu().ViewNextPerk();
	}
	
	public void ViewPreviousPerk()
	{
		MainGameMenu.Instance.PerksMenu().ViewPreviousPerk();
	}

	public void BuyPerk()
	{
		MainGameMenu.Instance.PerksMenu().BuyPerk();
	}

}
