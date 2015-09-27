using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUICanvas : MonoBehaviour 
{
	// snigleton
	private static GUICanvas instance = null;
	public static GUICanvas Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("GUICanvas");
				instance = thisObject.GetComponent<GUICanvas>();
			}
			return instance;
		}
	}

	private InGameGUICanvas mInGameGUICanvas;
	private MenuGUICanvas mMenuGUICanvas;
	private OptionsGUICanvas mOptionsGUICanvas;
	private bool mShowButtons;

	void Awake () 
	{
		mInGameGUICanvas = GameObject.Find("InGame GUICanvas").GetComponent<InGameGUICanvas>();
		mMenuGUICanvas = GameObject.Find("Menu GUICanvas").GetComponent<MenuGUICanvas>();
		mOptionsGUICanvas = GameObject.Find("Options GUICanvas").GetComponent<OptionsGUICanvas>();
	}

	// Use this for initialization
	void Start () 
	{
		ShowButtons (false);
	}
	
	// toggle menu buttons
	public void ShowMenuButtons(bool show)
	{
		mMenuGUICanvas.gameObject.SetActive (show);
	}
	
	// toggle options buttons
	public void ShowOptionButtons(bool show)
	{
		mOptionsGUICanvas.gameObject.SetActive (show);
	}
	
	// show ingame buttons
	public void ShowInGameButtons(bool show)
	{
		mInGameGUICanvas.gameObject.SetActive (show);
	}

	public void ToggleShowButtons ()
	{
		mShowButtons = !mShowButtons;
		UpdateButtons();
	}
	
	public void ShowButtons(bool show)
	{
		mShowButtons = show;
		UpdateButtons();
	}

	void UpdateButtons ()
	{
		ShowButtons(mInGameGUICanvas.GetButtons(), mShowButtons);
		ShowButtons(mOptionsGUICanvas.GetButtons(), mShowButtons);
		ShowButtons(mMenuGUICanvas.GetButtons(), mShowButtons);
	}

	public MenuGUICanvas MenuGUICanvas ()
	{
		return mMenuGUICanvas;
	}
	
	public InGameGUICanvas InGameGUICanvas ()
	{
		return mInGameGUICanvas;
	}

	public OptionsGUICanvas OptionsGUICanvas ()
	{
		return mOptionsGUICanvas;
	}

	public void ShowButtons(Button[] buttons, bool show)
	{
		float alpha = 0;
		if (show)
		{
			alpha = 1;
		}

		for (int i = 0; i < buttons.Length; i++) 
		{
			ColorBlock cb = buttons[i].colors;
			
			Color col = cb.highlightedColor;
			col.a = alpha;
			cb.highlightedColor = col;
			
			col = cb.normalColor;
			col.a = alpha;
			cb.normalColor = col;
			
			col = cb.pressedColor;
			col.a = alpha;
			cb.pressedColor = col;
			
			col = cb.disabledColor;
			col.a = alpha;
			cb.disabledColor = col;
			
			buttons[i].colors = cb;
		}
	}

	// Update is called once per frame
	void Update () 
	{
	}

	public void ToggleBloom()
	{
		MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
		InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
	}

	// other
	public GameObject GUIObject (string name)
	{
		switch (name) 
		{
		case "a":
			return null;
		}

		GameObject ret = MenuCamera.Instance.GUIObject(name);
		if (ret == null)
		{
			ret = MainGameMenu.Instance.GUIObject(name);
		}

		if (ret == null)
		{
			ret = InGame.Instance.GUIObject(name);
		}

		if (ret == null) 
		{
			print("ERORR! BUTTON OBJECT NOT FOUND: " + name);
		}

		return ret;
	}
}
