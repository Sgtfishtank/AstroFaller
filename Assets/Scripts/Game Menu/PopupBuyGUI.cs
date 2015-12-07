using UnityEngine;
using System.Collections;

public class PopupBuyGUI : GUICanvasBase
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

    // pressed popup buy buttons
    public void BuyWithBolts()
    {
        MainGameMenu.Instance.BuyWithBolts();
    }

    public void BuyWithCrystals()
    {
        MainGameMenu.Instance.BuyWithCrystals();
    }
}
