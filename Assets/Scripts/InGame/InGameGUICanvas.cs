using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InGameGUICanvas : MonoBehaviour 
{
	//private GameObject mInGameButtons;
	private GameObject mDeathMenu;
	public GameObject mRewardMenu;
	public GameObject mRewardTextMenu;
	
	private Image mFadeImage;

	private bool mShowButtons;
	private Button[] mButtons;
	private ButtonPress[] mButtonPresss;

	void Awake () 
	{
		mFadeImage = transform.Find ("FadeLayer").GetComponent<Image> ();

		//assign all option buttons
		mDeathMenu = GameObject.Find("DeathMenu");
		mDeathMenu.SetActive(false);
		
		//assign all in game buttons
		//mInGameButtons = transform.Find ("InGameButtons").gameObject;
		
		mButtonPresss = GetComponentsInChildren<ButtonPress>(true);
		mButtons = GetComponentsInChildren<Button>(true);
	}

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < mButtonPresss.Length; i++) 
		{
			mButtonPresss[i].Init();	
		}
	}
	
	// toggle menu buttons
	public void Deselect()
	{
		MainGameMenu.Instance.ResetAllMenusAndButtons ();
		MainGameMenu.Instance.UpdateMenusAndButtons ();
	}

	// Update is called once per frame
	void Update () 
	{
	
	}

	public Button[] GetButtons ()
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
		clear();
		WorldGen.Instance.Disable();
		MainGameMenu.Instance.Enable(0);
		setEnableDeathMenu(false);
		InGame.Instance.mDeathMenu.SetActive(false);
		InGame.Instance.DeathMenu().Close();
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
			value = UnityEngine.Random.Range(20,51);
			switch (pos)
			{
			case 1:
				a[0].text = value.ToString();
				a[0].rectTransform.anchoredPosition = findObject("Box 1").anchoredPosition + new Vector2(5.8f-128,-23);
				break;
			case 2:
				a[1].text = value.ToString();
				a[1].rectTransform.anchoredPosition = findObject("Box 2").anchoredPosition + new Vector2(3,-23);
				break;
			case 3:
				a[2].text = value.ToString();
				a[2].rectTransform.anchoredPosition = findObject("Box 3").anchoredPosition + new Vector2(2,-23);
				break;
			case 4:
				a[3].text = value.ToString();
				a[3].rectTransform.anchoredPosition = findObject("Box 4").anchoredPosition + new Vector2(3,-23);
				break;
			default:
				break;
			}
		}
		else
		{
			for (int i = 0; i < box; i++)
			{
				value += UnityEngine.Random.Range(20,51);
			}
			a[0].text = value.ToString();
			a[0].rectTransform.anchoredPosition = findObject("Box 1").anchoredPosition + new Vector2(5.8f - 75f,-23);
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
			if(b[i].name == name)
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
