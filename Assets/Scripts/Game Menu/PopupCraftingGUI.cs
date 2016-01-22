using UnityEngine;
using System.Collections;

public class PopupCraftingGUI : GUICanvasBase
{

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

    public void ChangeToPerksMenu()
    {
        MainGameMenu.Instance.ChangeToPerksMenu();
    }

    public void ChangeToItemsMenu()
    {
        MainGameMenu.Instance.ChangeToItemsMenu();
    }

    public void ChangeToChrystalShopMenu()
    {
        MainGameMenu.Instance.ChangeToChrystalShopMenu();
    }

}
