using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeatMenuGUI : GUICanvasBase 
{
	private GameObject mDeathMenu;
	private GameObject mRewardMenu;
	private GameObject mRewardTextMenu;
	private Text[] mTexts;

	void Awake()
	{
		mRewardMenu = transform.Find("Rewards").gameObject;
		mRewardTextMenu = transform.Find("RewardText").gameObject;
		//assign all option buttons
		mDeathMenu = gameObject;
		mTexts = mRewardTextMenu.GetComponentsInChildren<Text>();
		mDeathMenu.SetActive(false);
	}

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// pressed back to menu
	public void BackToMenu()
    {
        PlayerData.LoadScene(PlayerData.Scene.MAIN_MENU);
	}

	public void restart()
	{
		InGame.Instance.DeathMenu().Close();
		InGame.Instance.mDeathMenu.SetActive(false);

		InGame.Instance.StartGame();
	}

	// death
	public void setEnableDeathMenu(bool a)
	{
		mDeathMenu.SetActive(a);
		mRewardMenu.SetActive(a);
		mRewardTextMenu.SetActive(a);

		if (!a) 
		{
			clear();
		}
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
		for (int i = 0; i < mTexts.Length; i++)
		{
			mTexts[i].text = null;
		}
	}
}
