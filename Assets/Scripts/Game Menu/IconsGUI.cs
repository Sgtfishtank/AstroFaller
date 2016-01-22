using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IconsGUI : GUICanvasBase 
{
	private GameObject mWorldMapButton;

	void Awake()
	{
		mWorldMapButton = transform.Find("WorldMapButton").gameObject;
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
	
	public void ChangeToWorldMapMenu()
	{
		MainGameMenu.Instance.ChangeToWorldMapMenu();
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
	
	public void ShowWorldMapButton(bool show)
	{
		mWorldMapButton.gameObject.SetActive(show);
	}
}
