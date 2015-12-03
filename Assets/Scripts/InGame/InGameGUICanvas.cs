using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class InGameGUICanvas : MonoBehaviour 
{
	// snigleton
	private static InGameGUICanvas instance = null;
	public static InGameGUICanvas Instance
	{
		get
        {
            if (Application.loadedLevelName == "MainMenuLevel")
            {
                throw new NotImplementedException();
            }
            
			if (instance == null)
			{
                instance = Singleton<InGameGUICanvas>.CreateInstance("Prefab/Essential/InGame/InGameGUICanvas");
			}
			return instance;
		}
	}

	private GameObject mGUICanvasInGame;
	private bool mShowButtons;
	private DebugGUI mDebugGUI;

    //private GameObject mInGameButtons;
    private GameObject mDeathMenu;
    public GameObject mRewardMenu;
    public GameObject mRewardTextMenu;

    private Image mFadeImage;

    private Button[] mButtons;
    //private ButtonPress[] mButtonPresss;

	void Awake () 
	{
		mDebugGUI = GetComponent<DebugGUI>();
        mGUICanvasInGame = gameObject;
		mGUICanvasInGame.gameObject.SetActive (true);

        mFadeImage = transform.Find("FadeLayer").GetComponent<Image>();

        //assign all option buttons
        mDeathMenu = GameObject.Find("DeathMenu");
        mDeathMenu.SetActive(false);

        //assign all in game buttons
        //mInGameButtons = transform.Find ("InGameButtons").gameObject;

        //mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
        mButtons = GetComponentsInChildren<Button>(true);
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

    // toggle menu buttons
    public void Deselect()
    {
    }

    public Button[] GetButtons()
    {
        return mButtons;
    }

    public void SetFadeColor(Color col)
    {
        mFadeImage.color = col;
    }

    // pressed back to menu
    public void BackToMenu()
    {
        Application.LoadLevel("MainMenuLevel");
    }

    // death
    public void setEnableDeathMenu(bool a)
    {
        mDeathMenu.SetActive(a);
        mRewardMenu.SetActive(a);
        mRewardTextMenu.SetActive(a);
    }

    public void restart()
    {
        clear();
        InGame.Instance.mDeathMenu.SetActive(false);
        InGame.Instance.DeathMenu().Close();
        setEnableDeathMenu(false);
        InGame.Instance.StartGame();
    }

    public void perfectDistanceReward(int pos)
    {
        int box = InGame.Instance.Player().CollectedPerfectDistances();

        int value = 0;

        Text[] a = mRewardTextMenu.GetComponentsInChildren<Text>();

        if (box < 5)
        {
            value = UnityEngine.Random.Range(20, 51);
            switch (pos)
            {
                case 1:
                    a[0].text = value.ToString();
                    a[0].rectTransform.anchoredPosition = findObject("Box 1").anchoredPosition + new Vector2(5.8f - 128, -23);
                    break;
                case 2:
                    a[1].text = value.ToString();
                    a[1].rectTransform.anchoredPosition = findObject("Box 2").anchoredPosition + new Vector2(3, -23);
                    break;
                case 3:
                    a[2].text = value.ToString();
                    a[2].rectTransform.anchoredPosition = findObject("Box 3").anchoredPosition + new Vector2(2, -23);
                    break;
                case 4:
                    a[3].text = value.ToString();
                    a[3].rectTransform.anchoredPosition = findObject("Box 4").anchoredPosition + new Vector2(3, -23);
                    break;
                default:
                    break;
            }
        }
        else
        {
            for (int i = 0; i < box; i++)
            {
                value += UnityEngine.Random.Range(20, 51);
            }
            a[0].text = value.ToString();
            a[0].rectTransform.anchoredPosition = findObject("Box 1").anchoredPosition + new Vector2(5.8f - 75f, -23);
            mRewardMenu.GetComponent<DeathmenuButtons>().disableSpecific(5);
        }
        mRewardMenu.GetComponent<DeathmenuButtons>().disableSpecific(pos);
        InGame.Instance.mDeathMenu.GetComponent<DeathMenu>().removeBox(pos);
        PlayerData.Instance.depositBolts(value);
    }

    private RectTransform findObject(string name)
    {
        RectTransform[] b = mRewardMenu.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < b.Length; i++)
        {
            if (b[i].name == name)
            {
                return b[i];
            }
        }
        return null;
    }

    private void clear()
    {
        Text[] a = mRewardTextMenu.GetComponentsInChildren<Text>();
        for (int i = 0; i < a.Length; i++)
        {
            a[i].text = null;
        }
    }
}
