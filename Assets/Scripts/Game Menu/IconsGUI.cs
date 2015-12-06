using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconsGUI : MonoBehaviour 
{
	private ButtonPress mCraftingButton;
	private ButtonPress mOptionsButton;
	private ButtonPress mHelpButton;
	private ButtonPress mAchievementsButton;

	void Awake()
	{
		mCraftingButton = transform.Find ("CraftingButton").GetComponent<ButtonPress>();
		mOptionsButton = transform.Find ("OptionsButton").GetComponent<ButtonPress>();
		mHelpButton = transform.Find ("HelpButton").GetComponent<ButtonPress>();
		mAchievementsButton = transform.Find ("AchievementsButton").GetComponent<ButtonPress>();
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public ButtonPress WorkshopButton()
	{
		return mCraftingButton;
	}
	
	public ButtonPress SettingsButton()
	{
		return mOptionsButton;
	}	
	
	public ButtonPress OptionsButton()
	{
		return mHelpButton;
	}	
	
	public ButtonPress AchivementButton()
	{
		return mAchievementsButton;
	}
	
	// pressed icons gui buttons
	public void ToggleOptions()
	{
		MainGameMenu.Instance.ToggleOptions();
	}
	
	public void ToggleHelp()
	{
		MainGameMenu.Instance.ToggleHelp();
	}
	
	public void ToggleCraftingMenu()
	{
		MainGameMenu.Instance.ToggleCraftingMenu();
	}
	
	public void ToggleAchievementsMenu()
	{
		MainGameMenu.Instance.ToggleAchievementsMenu();
	}
}
