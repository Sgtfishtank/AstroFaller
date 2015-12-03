using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class GUICanvasMenu : MonoBehaviour 
{
	// snigleton
	private static GUICanvasMenu instance = null;
	public static GUICanvasMenu Instance
	{
		get
        {
            if (Application.loadedLevelName != "MainMenuLevel")
            {
                throw new NotImplementedException();
            }
            
			if (instance == null)
            {
                instance = Singleton<GUICanvasMenu>.CreateInstance("Prefab/Essential/Menu/GUICanvas");
			}
			return instance;
		}
	}

	private MenuGUICanvas mMenuGUICanvas;
	private OptionsGUICanvas mOptionsGUICanvas;
	private bool mShowButtons;
	private DebugGUI mDebugGUI;

	void Awake () 
	{
		mDebugGUI = GetComponent<DebugGUI>();
		mMenuGUICanvas = GetComponentsInChildren<MenuGUICanvas>(true)[0];
		mOptionsGUICanvas = GetComponentsInChildren<OptionsGUICanvas>(true)[0];
		mMenuGUICanvas.gameObject.SetActive (true);
		mOptionsGUICanvas.gameObject.SetActive (true);
	}

	// Use this for initialization
	void Start () 
	{
		mDebugGUI.enabled = false;
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
		ShowButtons(mOptionsGUICanvas.GetButtons(), mShowButtons);
		ShowButtons(mMenuGUICanvas.GetButtons(), mShowButtons);
	}

	public MenuGUICanvas MenuGUICanvas ()
	{
		return mMenuGUICanvas;
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
		Input.multiTouchEnabled = true;
		bool debugTouch = (Input.touchCount >= 4) && (Input.touches [3].phase == TouchPhase.Began);
		bool debugKey = (Input.GetKey(KeyCode.LeftControl)) && (Input.GetKeyDown(KeyCode.LeftShift));

		if (debugKey || debugTouch)
		{
			mDebugGUI.enabled = !mDebugGUI.enabled;
			
			if (!mDebugGUI.enabled)
			{
				GUICanvasMenu.Instance.ShowButtons(false);
				InGame.Instance.Player().mInvulnerable = false;
			}
		}

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

		/*if (ret == null)
		{
			ret = InGame.Instance.GUIObject(name);
		}*/

		if (ret == null) 
		{
			Debug.LogWarning("Warning! BUTTON OBJECT NOT FOUND: " + name);
		}

		return ret;
	}
}
