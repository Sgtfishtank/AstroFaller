using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GUICanvasInGame : MonoBehaviour 
{
	// snigleton
	private static GUICanvasInGame instance = null;
	public static GUICanvasInGame Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject thisObject = GameObject.Find("GUICanvas");
				instance = thisObject.GetComponent<GUICanvasInGame>();
			}
			return instance;
		}
	}

	private InGameGUICanvas mGUICanvasInGame;
	private bool mShowButtons;
	private DebugGUI mDebugGUI;

	void Awake () 
	{
		mDebugGUI = GetComponent<DebugGUI>();
		mGUICanvasInGame = GetComponentsInChildren<InGameGUICanvas>(true)[0];
		mGUICanvasInGame.gameObject.SetActive (true);
	}

	// Use this for initialization
	void Start () 
	{
		mDebugGUI.enabled = false;
		ShowButtons (false);
	}
	
	// show ingame buttons
	public void ShowInGameButtons(bool show)
	{
		mGUICanvasInGame.gameObject.SetActive (show);
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
		ShowButtons(mGUICanvasInGame.GetButtons(), mShowButtons);
	}
	
	public InGameGUICanvas GetGUICanvasInGame ()
	{
		return mGUICanvasInGame;
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
				GUICanvasInGame.Instance.ShowButtons(false);
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

		GameObject ret = null;

		if (ret == null)
		{
			ret = InGame.Instance.GUIObject(name);
		}

		if (ret == null) 
		{
			Debug.LogWarning("Warning! BUTTON OBJECT NOT FOUND: " + name);
		}

		return ret;
	}
}
