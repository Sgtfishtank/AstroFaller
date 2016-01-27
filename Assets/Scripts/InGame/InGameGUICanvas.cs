using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class InGameGUICanvas : GUICanvasBase 
{
	// snigleton
	private static InGameGUICanvas instance = null;
	public static InGameGUICanvas Instance
	{
		get
        {
			if (instance == null)
            {
                if (PlayerData.Instance.CurrentScene() != PlayerData.Scene.IN_GAME)
                {
                    throw new NotImplementedException();
                }

                instance = Singleton<InGameGUICanvas>.CreateInstance("Prefab/InGame/InGameGUICanvas");
			}
			return instance;
		}
	}

	private bool mShowButtons;
	private DebugGUI mDebugGUI;
	private DeatMenuGUI mDeathMenuGUI;
	private Image mFadeImage;
	private Button[] mButtons;

	void Awake () 
	{
		mDebugGUI = GetComponent<DebugGUI>();
        mFadeImage = transform.Find("FadeLayer").GetComponent<Image>();
		mDeathMenuGUI = transform.Find ("DeathMenu").GetComponent<DeatMenuGUI> ();
        //mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
        mButtons = GetComponentsInChildren<Button>(true);
	}

	// Use this for initialization
	void Start () 
	{
		gameObject.SetActive (true);
		mDebugGUI.enabled = false;
		ShowButtons (false);
	}
	
	// show ingame buttons
	public void ShowInGameButtons(bool show)
	{
		gameObject.SetActive(show);
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
        ShowButtons(mButtons, mShowButtons);
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
				InGameGUICanvas.Instance.ShowButtons(false);
				InGame.Instance.Player().mInvulnerable = false;
			}
		}

	}

	public void ToggleBloom()
	{
		//MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom>().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		//InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.Bloom> ().enabled;
		//MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !MenuCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
		//InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled = !InGameCamera.Instance.gameObject.GetComponent<UnityStandardAssets.ImageEffects.BloomOptimized> ().enabled;
	}

    // toggle menu buttons
    public void Deselect()
    {
		if (InGame.Instance.DeathMenu().IsOpen()) 
		{
			InGame.Instance.DeathMenu().Skip();
		}
    }

    public Button[] GetButtons()
    {
        return mButtons;
    }

    public void SetFadeColor(Color col)
    {
        mFadeImage.color = col;
    }

	public DeatMenuGUI DeathMenuGUI()
	{
		return mDeathMenuGUI;
	}
}
